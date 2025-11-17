using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class TestEvaluation
    {


        public int Id { get; set; }

        public int TestAttemptId { get; set; }
        public TestAttempt TestAttempt { get; set; } = null!;

        public float TotalScore { get; set; }
        public float MaxTotal { get; set; }
        public bool Passed { get; set; }
        public DateTime? EvaluatedAt { get; set; }


        public int Status { get; set; }


    }
}
