using HireAI.Data.Helpers.DTOs.AI;
using HireAI.Data.Helpers.Enums;
using HireAI.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HireAI.Service.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        
        // Rate limiting for free tier (15 requests per minute)
        private static readonly SemaphoreSlim _rateLimiter = new SemaphoreSlim(1, 1);
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private const int MinMillisecondsBetweenRequests = 4000; // ~15 requests per minute

        public GeminiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration["Gemini:ApiKey"];//?? "AIzaSyAK2GFNUaLZnr2szyuCOK-tPPZZbln1HcU";
        }

        public async Task<CVAnalysisResultDto> AnalyzeCVAsync(byte[] cvContent, string jobDescription, string fileName)
        {
            // Rate limiting
            await _rateLimiter.WaitAsync();
            try
            {
                var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
                if (timeSinceLastRequest.TotalMilliseconds < MinMillisecondsBetweenRequests)
                {
                    var delayMs = MinMillisecondsBetweenRequests - (int)timeSinceLastRequest.TotalMilliseconds;
                    await Task.Delay(delayMs);
                }

                var result = await CallGeminiApiAsync(cvContent, jobDescription, fileName);
                _lastRequestTime = DateTime.Now;
                return result;
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        private async Task<CVAnalysisResultDto> CallGeminiApiAsync(byte[] cvContent, string jobDescription, string fileName)
        {
            // Convert CV to base64 for Gemini API
            var base64Content = Convert.ToBase64String(cvContent);
            
            // Determine MIME type
            var mimeType = fileName.ToLower().EndsWith(".pdf") ? "application/pdf" : "application/octet-stream";

            var prompt = BuildATSPrompt(jobDescription);

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = mimeType,
                                    data = base64Content
                                }
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.4,
                    maxOutputTokens = 8192,
                    topP = 0.8,
                    topK = 40
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestUrl = $"{_apiUrl}?key={_apiKey}";
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API error: {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return ParseGeminiResponse(responseContent);
        }

        private string BuildATSPrompt(string jobDescription)
        {
            return $@"Analyze the CV against this job description and return ONLY a JSON response:

            Job: {jobDescription}

            Return JSON format:
            {{
              ""atsScore"": <0-100>,
              ""recommendedStatus"": ""<ATSPassed|ATSFailed>"",
              ""feedback"": ""<2 sentence summary>"",
              ""skillsFound"": [""skill1"", ""skill2""],
              ""skillsGaps"": [""gap1"", ""gap2""]
            }}

            Score: 70-100=ATSPassed, below 70=ATSFailed
            Match CV skills/experience to job requirements. Return ONLY the JSON.";
        }

        private CVAnalysisResultDto ParseGeminiResponse(string responseJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;
                
                // Check for error in response
                if (root.TryGetProperty("error", out var errorObj))
                {
                    var errorMessage = errorObj.TryGetProperty("message", out var msg) ? msg.GetString() : "Unknown error";
                    throw new InvalidOperationException($"Gemini API returned error: {errorMessage}");
                }

                // Extract text from Gemini response with defensive checks
                if (!root.TryGetProperty("candidates", out var candidatesArray) || candidatesArray.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException($"No candidates in response. Response: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}");
                }

                var firstCandidate = candidatesArray[0];
                
                // Check for finish reason issues
                if (firstCandidate.TryGetProperty("finishReason", out var finishReasonElem))
                {
                    var finishReason = finishReasonElem.GetString();
                    if (finishReason == "MAX_TOKENS")
                    {
                        throw new InvalidOperationException("Gemini response was truncated due to MAX_TOKENS limit. The AI output was incomplete.");
                    }
                    else if (finishReason == "SAFETY")
                    {
                        throw new InvalidOperationException("Gemini response was blocked due to safety filters.");
                    }
                    else if (finishReason != "STOP" && finishReason != null)
                    {
                        throw new InvalidOperationException($"Gemini response ended unexpectedly with finish reason: {finishReason}");
                    }
                }
                
                if (!firstCandidate.TryGetProperty("content", out var contentObj))
                {
                    throw new InvalidOperationException("No content in candidate response");
                }

                if (!contentObj.TryGetProperty("parts", out var partsArray) || partsArray.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException("No parts in content response. The AI may have been blocked or returned an incomplete response.");
                }

                var textPart = partsArray[0];
                
                if (!textPart.TryGetProperty("text", out var textElement))
                {
                    throw new InvalidOperationException("No text in parts response");
                }

                var generatedText = textElement.GetString() ?? "";

                // Clean up the text (remove markdown code blocks if present)
                generatedText = generatedText.Trim();
                if (generatedText.StartsWith("```json"))
                {
                    generatedText = generatedText.Substring(7);
                }
                if (generatedText.StartsWith("```"))
                {
                    generatedText = generatedText.Substring(3);
                }
                if (generatedText.EndsWith("```"))
                {
                    generatedText = generatedText.Substring(0, generatedText.Length - 3);
                }
                generatedText = generatedText.Trim();

                // Parse the JSON result
                using var resultDoc = JsonDocument.Parse(generatedText);
                var result = resultDoc.RootElement;

                // Parse with defaults if properties are missing
                var atsScore = result.TryGetProperty("atsScore", out var scoreElem) ? scoreElem.GetSingle() : 50f;
                var statusString = result.TryGetProperty("recommendedStatus", out var statusElem) ? statusElem.GetString() ?? "UnderReview" : "UnderReview";
                var feedback = result.TryGetProperty("feedback", out var feedbackElem) ? feedbackElem.GetString() ?? "No feedback provided" : "No feedback provided";

                var skillsFound = new List<string>();
                if (result.TryGetProperty("skillsFound", out var skillsFoundArray))
                {
                    foreach (var skill in skillsFoundArray.EnumerateArray())
                    {
                        var skillText = skill.GetString();
                        if (!string.IsNullOrWhiteSpace(skillText))
                        {
                            skillsFound.Add(skillText);
                        }
                    }
                }

                var skillsGaps = new List<string>();
                if (result.TryGetProperty("skillsGaps", out var skillsGapsArray))
                {
                    foreach (var skill in skillsGapsArray.EnumerateArray())
                    {
                        var skillText = skill.GetString();
                        if (!string.IsNullOrWhiteSpace(skillText))
                        {
                            skillsGaps.Add(skillText);
                        }
                    }
                }

                // Parse status enum - binary decision: pass or reject
                var recommendedStatus = statusString switch
                {
                    "ATSPassed" => enApplicationStatus.ATSPassed,
                    "ATSFailed" => enApplicationStatus.ATSFailed,
                    _ => atsScore >= 70 ? enApplicationStatus.ATSPassed : enApplicationStatus.ATSFailed
                };

                return new CVAnalysisResultDto
                {
                    AtsScore = atsScore,
                    RecommendedStatus = recommendedStatus,
                    Feedback = feedback,
                    SkillsFound = skillsFound,
                    SkillsGaps = skillsGaps
                };
            }
            catch (JsonException jsonEx)
            {
                throw new InvalidOperationException($"Failed to parse JSON response. Error: {jsonEx.Message}. Response snippet: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}", jsonEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse Gemini response: {ex.Message}. Response snippet: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}", ex);
            }
        }

        public async Task<AIGeneratedQuestionsResponseDto> GenerateJobExamQuestionsAsync(string jobDescription)
        {
            // Rate limiting
            await _rateLimiter.WaitAsync();
            try
            {
                var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
                if (timeSinceLastRequest.TotalMilliseconds < MinMillisecondsBetweenRequests)
                {
                    var delayMs = MinMillisecondsBetweenRequests - (int)timeSinceLastRequest.TotalMilliseconds;
                    await Task.Delay(delayMs);
                }

                var result = await GenerateQuestionsFromDescriptionAsync(jobDescription, "job");
                _lastRequestTime = DateTime.Now;
                return result;
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        public async Task<AIGeneratedQuestionsResponseDto> GenerateMockExamQuestionsAsync(string examDescription)
        {
            // Rate limiting
            await _rateLimiter.WaitAsync();
            try
            {
                var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
                if (timeSinceLastRequest.TotalMilliseconds < MinMillisecondsBetweenRequests)
                {
                    var delayMs = MinMillisecondsBetweenRequests - (int)timeSinceLastRequest.TotalMilliseconds;
                    await Task.Delay(delayMs);
                }

                var result = await GenerateQuestionsFromDescriptionAsync(examDescription, "mock exam");
                _lastRequestTime = DateTime.Now;
                return result;
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        private async Task<AIGeneratedQuestionsResponseDto> GenerateQuestionsFromDescriptionAsync(string description, string examType)
        {
            var prompt = BuildMCQPrompt(description, examType);

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 4096,
                    topP = 0.9,
                    topK = 40
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestUrl = $"{_apiUrl}?key={_apiKey}";
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API error: {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return ParseQuestionsResponse(responseContent);
        }

        private string BuildMCQPrompt(string description, string examType)
        {
            return $@"Generate 10 multiple-choice questions (MCQ) for a {examType} based on the following description.

            Description: {description}

            Requirements:
            1. Generate exactly 10 questions
            2. Each question must have exactly 4 choices
            3. Each question must have one correct answer
            4. Questions should be relevant to the description
            5. Make questions challenging but fair
            6. Index the correct answer (0-3)

            Return ONLY a JSON response in this exact format:
            {{
              ""questions"": [
                {{
                  ""questionText"": ""<question text>"",
                  ""choices"": [""<choice 0>"", ""<choice 1>"", ""<choice 2>"", ""<choice 3>""],
                  ""correctAnswerIndex"": <0-3>
                }}
              ]
            }}

            Return ONLY the JSON, no additional text or markdown.";
        }

        private AIGeneratedQuestionsResponseDto ParseQuestionsResponse(string responseJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;

                // Check for error in response
                if (root.TryGetProperty("error", out var errorObj))
                {
                    var errorMessage = errorObj.TryGetProperty("message", out var msg) ? msg.GetString() : "Unknown error";
                    throw new InvalidOperationException($"Gemini API returned error: {errorMessage}");
                }

                // Extract text from Gemini response
                if (!root.TryGetProperty("candidates", out var candidatesArray) || candidatesArray.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException($"No candidates in response. Response: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}");
                }

                var firstCandidate = candidatesArray[0];

                // Check for finish reason issues
                if (firstCandidate.TryGetProperty("finishReason", out var finishReasonElem))
                {
                    var finishReason = finishReasonElem.GetString();
                    if (finishReason == "MAX_TOKENS")
                    {
                        throw new InvalidOperationException("Gemini response was truncated due to MAX_TOKENS limit.");
                    }
                    else if (finishReason == "SAFETY")
                    {
                        throw new InvalidOperationException("Gemini response was blocked due to safety filters.");
                    }
                }

                if (!firstCandidate.TryGetProperty("content", out var contentObj) ||
                    !contentObj.TryGetProperty("parts", out var partsArray) ||
                    partsArray.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException("Invalid response structure from Gemini API");
                }

                var textPart = partsArray[0];
                if (!textPart.TryGetProperty("text", out var textElement))
                {
                    throw new InvalidOperationException("No text in response");
                }

                var generatedText = textElement.GetString() ?? "";

                // Clean up markdown code blocks if present
                generatedText = generatedText.Trim();
                if (generatedText.StartsWith("```json"))
                {
                    generatedText = generatedText.Substring(7);
                }
                if (generatedText.StartsWith("```"))
                {
                    generatedText = generatedText.Substring(3);
                }
                if (generatedText.EndsWith("```"))
                {
                    generatedText = generatedText.Substring(0, generatedText.Length - 3);
                }
                generatedText = generatedText.Trim();

                // Parse the JSON result
                using var resultDoc = JsonDocument.Parse(generatedText);
                var result = resultDoc.RootElement;

                var questionsResponse = new AIGeneratedQuestionsResponseDto();

                if (result.TryGetProperty("questions", out var questionsArray))
                {
                    foreach (var questionElem in questionsArray.EnumerateArray())
                    {
                        var questionText = questionElem.TryGetProperty("questionText", out var qtElem) ? qtElem.GetString() ?? "" : "";
                        var correctAnswerIndex = questionElem.TryGetProperty("correctAnswerIndex", out var caElem) ? caElem.GetInt32() : 0;

                        var choices = new List<string>();
                        if (questionElem.TryGetProperty("choices", out var choicesArray))
                        {
                            foreach (var choiceElem in choicesArray.EnumerateArray())
                            {
                                var choice = choiceElem.GetString();
                                if (!string.IsNullOrWhiteSpace(choice))
                                {
                                    choices.Add(choice);
                                }
                            }
                        }

                        // Ensure we have exactly 4 choices
                        while (choices.Count < 4)
                        {
                            choices.Add($"Option {choices.Count + 1}");
                        }

                        questionsResponse.Questions.Add(new AIGeneratedQuestionDto
                        {
                            QuestionText = questionText,
                            Choices = choices.Take(4).ToList(),
                            CorrectAnswerIndex = Math.Clamp(correctAnswerIndex, 0, 3)
                        });
                    }
                }

                // Ensure we have at least some questions
                if (questionsResponse.Questions.Count == 0)
                {
                    throw new InvalidOperationException("No questions were generated by Gemini API");
                }

                return questionsResponse;
            }
            catch (JsonException jsonEx)
            {
                throw new InvalidOperationException($"Failed to parse JSON response. Error: {jsonEx.Message}. Response snippet: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}", jsonEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse Gemini response: {ex.Message}. Response snippet: {responseJson.Substring(0, Math.Min(500, responseJson.Length))}", ex);
            }
        }
    }
}

