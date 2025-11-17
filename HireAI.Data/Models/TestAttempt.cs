using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HireAI.Data.Models
{
    public class TestAttempt
    {

       
        public int Id { get; set; }

        public int JobId { get; set; }
        public JobOpening Job { get; set; } = null!;

        public int ApplicationId { get; set; }
        public Application Application { get; set; } = null!;

        public int TestId { get; set; }
        public Exam Test { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      
        public ICollection<ApplicantResponse>? ApplicantResponses { get; set; }
        public TestEvaluation? TestEvaluation { get; set; }
    

    }
}
