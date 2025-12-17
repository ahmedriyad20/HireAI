using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Application
{
    public class CreateApplicationDto
    {
        public int ApplicantId { get; set; }
        public int JobId { get; set; }
        public int? HRId { get; set; }
        public string? CVFilePath { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; } = enApplicationStatus.UnderReview;
    }
}