

namespace HireAI.Data.Models
{
    public class ExamSummary
    {

        public int Id { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public float ApplicantExamScore { get; set; }

        //Foreign Keys
        public int ApplicationId { get; set; }
        public int ExamId { get; set; }
        public int? ExamEvaluationId { get; set; }

        //Navigation Property
        public Application Application { get; set; } = null!;
        public Exam Exam { get; set; } = null!;
        public ExamEvaluation? ExamEvaluation { get; set; }

    }
}
