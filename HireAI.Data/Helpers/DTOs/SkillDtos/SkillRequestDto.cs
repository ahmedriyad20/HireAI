using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.SkillDtos
{
    /// <summary>
    /// DTO used to create or update a Skill.
    /// Contains skill details for API requests.
    /// </summary>
    public class SkillRequestDto
    {
        [Required(ErrorMessage = "Skill title is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Skill title must be between 1 and 100 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Skill description must not exceed 500 characters.")]
        public string? Description { get; set; }
    }
}