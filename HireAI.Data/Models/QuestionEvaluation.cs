using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class QuestionEvaluation
    {
        public Guid Id { get; set; }

        public Guid ResponseId { get; set; } // Foreign Key to Response

        public bool IsvCorrect { get; set; } = false;

        public  string Feedback { get; set; } = null!;

        public DateTime EvaluatedAt { get; set; }
    }
}
