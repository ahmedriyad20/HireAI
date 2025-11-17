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
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TestId { get; set; }
        public string CreatedAt { get; set; }

    }
}
