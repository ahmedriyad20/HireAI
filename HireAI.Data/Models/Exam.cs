using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    //just to upload this for the team 
    public class Exam
    {
        public int Id { get; set; }
        public int NumberOfQuestions { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ExamName { get; set; } = default!;
        public bool IsAi { get; set; } = true;
        public enExamType ExamType { get; set; } = enExamType.MockExam;

        //Foreign Keys
        public int? ApplicantId { get; set; }
        public int? ApplicationId { get; set; }

        // Navigation Property
        public Applicant? Applicant { get; set; }
        public Application? Application { get; set; }
        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();  

    }
}