using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    internal class CVs
    {
        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? LinkedInPath { get; set; }
        public string? GitHubPath { get; set; }
        public string? Title { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public float? YearOfExperience { get; set; }
        public string? Certifications { get; set; }

        // Foreign Key
        public Guid CandidatedId { get; set; }
    }
}
