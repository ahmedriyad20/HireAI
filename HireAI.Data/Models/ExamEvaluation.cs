using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class ExamEvaluation
    {
        public int Id { get; set; }
        public float TotalScore { get; set; }
        public float MaxTotal { get; set; }
        public bool IsPassed { get; set; }
        public DateTime? EvaluatedAt { get; set; }
        public enExamEvaluationStatus Status { get; set; } = enExamEvaluationStatus.Pending;

        //Foreign Keys
        public int ExamSummaryId { get; set; }
        public int JobId { get; set; }

        //Navigation Property
        public ExamSummary? ExamSummary { get; set; }
        public JobPost? JobPost { get; set; }
        public ICollection<QuestionEvaluation> QuestionEvaluations { get; set; } = new HashSet<QuestionEvaluation>();
    }
}
