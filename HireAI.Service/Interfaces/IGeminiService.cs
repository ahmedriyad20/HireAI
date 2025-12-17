using HireAI.Data.Helpers.DTOs.AI;

namespace HireAI.Service.Interfaces
{
    public interface IGeminiService
    {
        /// <summary>
        /// Analyzes CV content against job description using Gemini AI
        /// Returns ATS score and recommendation
        /// </summary>
        Task<CVAnalysisResultDto> AnalyzeCVAsync(byte[] cvContent, string jobDescription, string fileName);

        /// <summary>
        /// Generates 10 MCQ questions based on job description using Gemini AI
        /// </summary>
        Task<AIGeneratedQuestionsResponseDto> GenerateJobExamQuestionsAsync(string jobDescription);

        /// <summary>
        /// Generates 10 MCQ questions for mock exam based on exam description using Gemini AI
        /// </summary>
        Task<AIGeneratedQuestionsResponseDto> GenerateMockExamQuestionsAsync(string examDescription);
    }
}

