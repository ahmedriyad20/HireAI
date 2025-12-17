using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Application
{
    public class ApplicationResponseDto
    {
        public int Id { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime DateApplied { get; set; }
        public string? CVFilePath { get; set; }
        public float? AtsScore { get; set; }
        public enExamStatus ExamStatus { get; set; }

        // Foreign Keys
        public int? HRId { get; set; }
        public int? ApplicantId { get; set; }
        public int? JobId { get; set; }
        public int? ExamId { get; set; }

        // Optional: Include related entity names if needed
        public string? ApplicantName { get; set; }
        public string? JobTitle { get; set; }
        public string? HRName { get; set; }
    }
}