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

        public int ResponseId { get; set; } // Foreign Key to Response

        public bool IsCorrect { get; set; } = false;

        public  string Feedback { get; set; } = null!;

        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
    }
}
