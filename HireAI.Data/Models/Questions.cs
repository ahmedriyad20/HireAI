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
        public int Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public enQuestionAnswers? Answer { get; set; }
        public int QuestionNumber { get; set; }

        //Foreign Keys
        public int ExamId { get; set; }
        public int? ApplicantResponseId { get; set; }

        //Navigation Property
        public ApplicantResponse? ApplicantResponse { get; set; }
        public Exam? Exam { get; set; }
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}
