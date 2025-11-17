using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class CandidateCVSkill
    {
        public int Id { get; set; }
        public int CVId { get; set; }
        public string SkillName { get; set; }
    }
}
