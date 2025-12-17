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
        public string ExamName { get; set; } = default!;
        public string ExamDescription { get; set; } = default!;
        public enExamLevel ExamLevel { get; set; } = enExamLevel.Beginner;
        public int NumberOfQuestions { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAi { get; set; } = true;
        public enExamType ExamType { get; set; } = enExamType.MockExam;

        // Navigation Property
        public ICollection<Applicant> Applicants { get; set; } = new HashSet<Applicant>();
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();  

    }
}