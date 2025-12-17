using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Applicant
{
    public class AddApplicantSkillsRequestDto
    {
        [Required(ErrorMessage = "Skill IDs are required")]
        [MinLength(1, ErrorMessage = "At least one skill ID must be provided")]
        public List<int> SkillIds { get; set; } = new List<int>();
    }
}
