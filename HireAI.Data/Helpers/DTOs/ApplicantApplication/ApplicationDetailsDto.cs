using HireAI.Data.Helpers.Enums;


namespace HireAI.Data.Helpers.DTOs.ApplicantApplication
{
    public class ApplicationDetailsDto
    {
        public int ApplicationId { get; set; }
        public string JobTitle { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string CompanyLocation { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? IntrviewDate { get; set; } = DateTime.UtcNow;
        public enExperienceLevel ExperienceLevel { get; set; } = enExperienceLevel.EntryLevel;
        public string SalaryRange { get; set; } = default!;
        public int? ExamScore { get; set; }
        public int NumberOfApplicants { get; set; }
        public float? AtsScore { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public enExamEvaluationStatus ExamEvaluationStatus { get; set; } = enExamEvaluationStatus.Pending;
    }
}
