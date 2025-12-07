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
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        //add to solve circular reference issue
        public ICollection<ApplicantSkill> ApplicantSkills { get; set; } = new HashSet<ApplicantSkill>();

    }
}
