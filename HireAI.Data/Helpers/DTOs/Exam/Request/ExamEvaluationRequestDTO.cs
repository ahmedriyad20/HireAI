using HireAI.Data.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Exam.Request
{
    public class ExamEvaluationRequestDTO
    {
        //[Required(ErrorMessage = "Application ID is required")]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Exam ID is required")]
        public int ExamId { get; set; }

        //[Required(ErrorMessage = "Job ID is required")]
        public int JobId { get; set; }

        public int? ApplicantId { get; set; }

        [Required(ErrorMessage = "Applicant exam score is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Score cannot be negative")]
        public float ApplicantExamScore { get; set; }

        public float? ExamTotalScore { get; set; } = 100;

        public DateTime? AppliedAt { get; set; } = DateTime.Now;

        public enExamEvaluationStatus Status { get; set; } = enExamEvaluationStatus.Pending;
    }
}
