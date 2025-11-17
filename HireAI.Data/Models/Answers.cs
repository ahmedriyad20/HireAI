using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Answers
    {
        public Guid Id { get; set; } 
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;

        public Guid QuestionId { get; set; } // Foreign Key to Question
        public Question Question { get; set; } = null!;
    }
}
