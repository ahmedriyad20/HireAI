using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Applicant: User
    {
        public string ResumeUrl { get; set; } = null!;
       
    }
}
