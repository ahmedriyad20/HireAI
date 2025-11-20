using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class CV
    {
        public int Id { get; set; }
        public string? Phone { get; set; }
        public string? LinkedInPath { get; set; }
        public string? GitHubPath { get; set; }
        public string? Title { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public float? YearsOfExperience { get; set; }
        public List<string>? Certifications { get; set; }

        //Foreign Keys
        public int ApplicantId { get; set; }

        // navigation back to Applicant
        public virtual Applicant? Applicant { get; set; }
    }
}
