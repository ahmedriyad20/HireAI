using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public  class Skill
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        
        public ICollection<JobSkill>? JobSkills { get; set; }
        public ICollection<ApplicantSkill>? ApplicantSkills { get; set; }
    }
}
