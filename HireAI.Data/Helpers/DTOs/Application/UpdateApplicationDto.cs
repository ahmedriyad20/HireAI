using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Application
{
    public class UpdateApplicationDto
    {
        public int Id { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public float? AtsScore { get; set; }
        public int? ExamId { get; set; }
        public enExamStatus ExamStatus { get; set; }
        public string? CVFilePath { get; set; }
    }
}