using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.SkillDtos
{
    /// <summary>
    /// DTO used to return a Skill response.
    /// Contains skill details for API responses.
    /// </summary>
    public class SkillResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}