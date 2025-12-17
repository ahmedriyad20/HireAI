using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.AI
{
    public class CVAnalysisResultDto
    {
        /// <summary>
        /// ATS Score from 0-100
        /// </summary>
        public float AtsScore { get; set; }

        /// <summary>
        /// Recommended application status based on analysis
        /// </summary>
        public enApplicationStatus RecommendedStatus { get; set; }

        /// <summary>
        /// Detailed feedback from AI analysis
        /// </summary>
        public string Feedback { get; set; } = string.Empty;

        /// <summary>
        /// Key skills found in CV
        /// </summary>
        public List<string> SkillsFound { get; set; } = new();

        /// <summary>
        /// Skills missing from job requirements
        /// </summary>
        public List<string> SkillsGaps { get; set; } = new();
    }
}

