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
        
       
        public int AnswerNumber { get; set; }

        //Foreign Keys
        public int QuestionId { get; set; }
        public int ExamSummaryId { get; set; }


        // Navigation
        public QuestionEvaluation? QuestionEvaluation { get; set; }
        public Question Question { get; set; } = null!;
        public ExamSummary ExamSummary { get; set; } = null!;

    }
}
