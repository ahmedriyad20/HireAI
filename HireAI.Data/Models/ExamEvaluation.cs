using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Models
{
    public class ExamEvaluation
    {
        public int Id { get; set; }
        public float ApplicantExamScore { get; set; }
        public float ExamTotalScore { get; set; }
        public DateTime? EvaluatedAt { get; set; }
        public enExamEvaluationStatus Status { get; set; } = enExamEvaluationStatus.Pending;

        //Foreign Keys
        public int ExamSummaryId { get; set; }
        public int? JobId { get; set; }

        //Navigation Property
        public ExamSummary? ExamSummary { get; set; }
        public JobPost? JobPost { get; set; }
        public ICollection<QuestionEvaluation> QuestionEvaluations { get; set; } = new HashSet<QuestionEvaluation>();
    }
}
