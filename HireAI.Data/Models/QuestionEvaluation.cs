using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class QuestionEvaluation
    {
        public int Id { get; set; }
        public bool IsCorrect { get; set; } = false;
        public  string Feedback { get; set; } = null!;

        //Foreign Keys
        public int? ApplicantResponseId { get; set; } // Foreign Key to Response
        public int? ExamEvaluationId { get; set; }

        //Navigation Property
        public ApplicantResponse? ApplicantResponse { get; set; }
        public ExamEvaluation? ExamEvaluation { get; set; }

    }
}
