using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Exam
{
    public class MockExamDto
    {
        public string ExamName { get; set; } = default!;
        public string ExamDescription { get; set; } = default!;
        public enExamLevel ExamLevel { get; set; } = enExamLevel.Beginner;
        public int NumberOfQuestions { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
