using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public  class JobSkill
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public JobOpening Job { get; set; } = null!;

        public int SkillId { get; set; }

        public Skill Skill { get; set; } = null!;
    }
}
