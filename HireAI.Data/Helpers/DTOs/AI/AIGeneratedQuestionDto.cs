using System.Collections.Generic;

namespace HireAI.Data.Helpers.DTOs.AI
{
    /// <summary>
    /// DTO for AI-generated questions from Gemini API
    /// </summary>
    public class AIGeneratedQuestionDto
    {
        public string QuestionText { get; set; } = default!;
        public List<string> Choices { get; set; } = new List<string>();
        public int CorrectAnswerIndex { get; set; }
    }

    /// <summary>
    /// Response DTO containing multiple AI-generated questions
    /// </summary>
    public class AIGeneratedQuestionsResponseDto
    {
        public List<AIGeneratedQuestionDto> Questions { get; set; } = new List<AIGeneratedQuestionDto>();
    }
}

