# AI Exam Generation Implementation

## Overview
This document describes the implementation of two new endpoints that use Google's Gemini 2.5 Flash AI to automatically generate multiple-choice questions (MCQ) for job exams and mock exams.

## Features Implemented

### 1. CreateJobExamAsync Endpoint
**Route:** `POST /api/Exam/job-exam/{applicationId}`

**Description:** Creates an exam with AI-generated questions based on a job post's description.

**Flow:**
1. Accepts an `applicationId` parameter
2. Retrieves the application from the database
3. Fetches the associated job post
4. Extracts the job description
5. Sends the description to Gemini AI with a professionally crafted prompt
6. Receives 10 MCQ questions with 4 choices each
7. Creates an Exam entity with type `HrExam`
8. Stores all questions in the database
9. Links the exam to the application
10. Returns the generated questions as JSON

**Example Request:**
```http
POST /api/Exam/job-exam/123
Authorization: Bearer {token}
```

**Example Response:**
```json
{
  "message": "Job exam created successfully with AI-generated questions",
  "questionCount": 10,
  "questions": [
    {
      "id": 1,
      "questionText": "What is polymorphism in OOP?",
      "questionNumber": 1,
      "examId": 456
    },
    // ... 9 more questions
  ]
}
```

### 2. CreateMockExamAsync Endpoint
**Route:** `POST /api/Exam/mock-exam/{examId}`

**Description:** Generates AI-powered questions for an existing mock exam based on its description.

**Flow:**
1. Accepts an `examId` parameter
2. Retrieves the exam from the database
3. Extracts the exam description
4. Sends the description to Gemini AI
5. Receives 10 MCQ questions
6. Stores questions in the database linked to the exam
7. Updates the exam's question count
8. Returns the generated questions as JSON

**Example Request:**
```http
POST /api/Exam/mock-exam/789
Authorization: Bearer {token}
```

**Example Response:**
```json
{
  "message": "Mock exam questions generated successfully",
  "questionCount": 10,
  "questions": [
    {
      "id": 10,
      "questionText": "Which data structure uses LIFO principle?",
      "questionNumber": 1,
      "examId": 789
    },
    // ... 9 more questions
  ]
}
```

## Architecture & Files Created/Modified

### New Files Created

#### 1. `HireAI.Data/Helpers/DTOs/AI/AIGeneratedQuestionDto.cs`
```csharp
// DTOs for AI-generated questions
- AIGeneratedQuestionDto: Individual question with choices and correct answer
- AIGeneratedQuestionsResponseDto: Collection of questions from AI
```

### Modified Files

#### 2. `HireAI.Service/Interfaces/IGeminiService.cs`
Added two new methods:
- `GenerateJobExamQuestionsAsync(string jobDescription)`
- `GenerateMockExamQuestionsAsync(string examDescription)`

#### 3. `HireAI.Service/Services/GeminiService.cs`
Implemented AI question generation with:
- **Rate limiting:** Respects free tier limits (15 requests/min, ~4s between requests)
- **Token optimization:** Uses temperature=0.7, maxOutputTokens=4096
- **Robust parsing:** Handles markdown code blocks, safety filters, token limits
- **Professional prompts:** Crafted to generate exactly 10 MCQ questions with 4 choices each

**Key Features:**
```csharp
// Rate limiting implementation
private static readonly SemaphoreSlim _rateLimiter = new SemaphoreSlim(1, 1);
private const int MinMillisecondsBetweenRequests = 4000; // ~15 req/min

// Optimized generation config for free tier
generationConfig = new
{
    temperature = 0.7,      // Good balance of creativity/consistency
    maxOutputTokens = 4096, // Sufficient for 10 questions
    topP = 0.9,
    topK = 40
}
```

#### 4. `HireAI.Service/Interfaces/IExamService.cs`
Added two new interface methods:
- `CreateJobExamAsync(int applicationId)`
- `CreateMockExamAsync(int examId)`

#### 5. `HireAI.Service/Services/ExamService.cs`
Implemented business logic:
- Dependency injection of repositories and Gemini service
- Database transaction handling
- Exam creation with appropriate metadata
- Question persistence
- Error handling and validation

**Key Logic:**
```csharp
// Maps job experience level to exam difficulty
ExamLevel = jobPost.ExperienceLevel switch
{
    enExperienceLevel.EntryLevel => enExamLevel.Beginner,
    enExperienceLevel.Junior => enExamLevel.Beginner,
    enExperienceLevel.MidLevel => enExamLevel.Intermediate,
    enExperienceLevel.Senior => enExamLevel.Advanced,
    // ...
}
```

#### 6. `HireAI.API/Controllers/ExamController.cs`
Added two new endpoints with:
- Proper HTTP method attributes (`[HttpPost]`)
- Route templates with parameters
- Response type documentation
- Error handling with try-catch
- Descriptive success messages

## Technical Details

### Rate Limiting Strategy
To comply with Gemini's free tier limits (15 requests per minute):
```csharp
// Enforces ~4 seconds between requests
await _rateLimiter.WaitAsync();
var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
if (timeSinceLastRequest.TotalMilliseconds < MinMillisecondsBetweenRequests)
{
    await Task.Delay(delayMs);
}
```

### Token Optimization
- **Temperature:** 0.7 for balanced creativity
- **Max Tokens:** 4096 (sufficient for 10 questions, well under free tier limits)
- **Prompt Engineering:** Clear, concise instructions to minimize input tokens

### AI Prompt Structure
```
Generate 10 multiple-choice questions (MCQ) for a {examType} based on the following description.

Description: {description}

Requirements:
1. Generate exactly 10 questions
2. Each question must have exactly 4 choices
3. Each question must have one correct answer
4. Questions should be relevant to the description
5. Make questions challenging but fair
6. Index the correct answer (0-3)

Return ONLY a JSON response in this exact format:
{
  "questions": [
    {
      "questionText": "<question text>",
      "choices": ["<choice 0>", "<choice 1>", "<choice 2>", "<choice 3>"],
      "correctAnswerIndex": <0-3>
    }
  ]
}
```

### Error Handling
Both endpoints include comprehensive error handling:
- Application/Exam not found
- Missing job/exam description
- Gemini API errors (safety filters, token limits)
- JSON parsing errors
- Database errors

## Database Schema

### Question Model
```csharp
public class Question
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string[] Choices { get; set; } // 4 choices
    public int QuestionNumber { get; set; }
    public int CorrectAnswerIndex { get; set; } // 0-3
    public int? ExamId { get; set; }
    
    // Navigation Properties
    public Exam? Exam { get; set; }
}
```

### Exam Model
```csharp
public class Exam
{
    public int Id { get; set; }
    public string ExamName { get; set; }
    public string ExamDescription { get; set; }
    public enExamLevel ExamLevel { get; set; }
    public int NumberOfQuestions { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAi { get; set; }
    public enExamType ExamType { get; set; } // MockExam or HrExam
    
    // Navigation Properties
    public ICollection<Question> Questions { get; set; }
    public ICollection<Application> Applications { get; set; }
}
```

## Authorization
Both endpoints require authentication with `[Authorize(Roles = "HR,Applicant")]` at the controller level.

## Configuration
Gemini API key is stored in `appsettings.json`:
```json
{
  "Gemini": {
    "ApiKey": "AIzaSyAK2GFNUaLZnr2szyuCOK-tPPZZbln1HcU"
  }
}
```

## Testing Recommendations

### 1. Test Job Exam Creation
```bash
curl -X POST "http://localhost:5290/api/Exam/job-exam/1" \
  -H "Authorization: Bearer {token}"
```

### 2. Test Mock Exam Creation
```bash
curl -X POST "http://localhost:5290/api/Exam/mock-exam/5" \
  -H "Authorization: Bearer {token}"
```

### 3. Test Error Cases
- Invalid application ID (should return 404)
- Application without job (should return 400)
- Job without description (should return 400)
- Exam without description (should return 400)

## Performance Considerations

1. **Rate Limiting:** Automatic 4-second delays between requests
2. **Token Usage:** Optimized prompts to minimize costs
3. **Database Efficiency:** Batch inserts for questions
4. **Async Operations:** All database and API calls are async

## Future Enhancements

1. **Question Quality Validation:** Add AI-powered validation of generated questions
2. **Difficulty Tuning:** Allow custom difficulty settings per exam
3. **Multi-language Support:** Generate questions in different languages
4. **Question Bank:** Cache and reuse similar questions
5. **Custom Question Count:** Allow variable number of questions (not just 10)
6. **Retry Logic:** Implement exponential backoff for API failures

## Build Status
✅ **Build Successful** - All files compile without errors

## Dependencies
- Gemini 2.5 Flash API (Free Tier)
- Entity Framework Core
- AutoMapper
- ASP.NET Core 9.0
- System.Text.Json

## Notes
- The implementation follows the existing project architecture
- All services are registered in DI container
- Rate limiting ensures compliance with free tier limits
- Questions are stored with correct answer indices (0-3)
- Both endpoints return detailed success/error messages

