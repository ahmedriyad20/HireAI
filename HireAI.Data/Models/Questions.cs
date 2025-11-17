using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; } //foreign key to Test
        public string QuestionText { get; set; } = null!;
        public QuestionAnswers? Answer { get; set; }

        public int QuestionNumber { get; set; }
        private Question() { }
        public Question(Guid testId, string questionText, QuestionAnswers  answer )
        {
            TestId = testId;
            QuestionText = questionText;
            Answer = answer;
        }   
    }
}
