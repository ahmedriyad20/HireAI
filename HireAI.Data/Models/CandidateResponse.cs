using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HireAI.Data.Models
{

    public class ApplicantResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int TestAttemptId { get; set; }
        public ExamSummary TestAttempt { get; set; } = null!;
        public int AnswerNumber { get; set; }

        // Navigation
        public QuestionEvaluation? QuestionEvaluation { get; set; }

    }
}
