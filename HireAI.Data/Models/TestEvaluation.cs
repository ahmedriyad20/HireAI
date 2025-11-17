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
        public Guid Id { get; set; }
        public Guid TestAttemptId { get; set; }
        public float TotalScore { get; set; }
        public float MaxTotal { get; set; }
        public bool Passed { get; set; }
        public int Status { get; set; }
        public int EvaluatedAt { get; set; }
    }
}
