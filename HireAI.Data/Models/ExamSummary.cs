using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HireAI.Data.Models
{
    public class ExamSummary
    {

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public float TotalScroe { get; set; }
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
