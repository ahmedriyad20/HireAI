using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Applicant
{
    public class ApplicantSkillDto
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; } = default!;
        public float? SkillRate { get; set; }
        public float? ImprovementPercentage { get; set; }
        public string? Notes { get; set; }
    }
}
