using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public   class ApplicantSkill
    {
        public int Id { get; set; }
        public float? SkillRate { get; set; }

        //Foreign Keys
        public int ApplicantId { get; set; }
        public int SkillId { get; set; }

        //Navigation Property
        public Applicant? Applicant { get; set; }
        public Skill? Skill { get; set; }
        //public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();

    }
}
