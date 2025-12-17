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

        //Foreign Keys
        public int JobId { get; set; }
        public int SkillId { get; set; }

        //Naviagation Property
        public JobPost? Job { get; set; }
        public Skill? Skill { get; set; } 
    }
}
