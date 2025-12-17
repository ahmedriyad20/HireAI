using HireAI.Data.Helpers.DTOs.SkillDtos;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSkills()
        {
            var skills = await _skillService.GetAllSkillsAsync();
            return Ok(skills);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            if (skill == null)
                return NotFound(new { message = $"Skill with ID {id} not found." });

            return Ok(skill);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSkill([FromBody] SkillRequestDto skillRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdSkill = await _skillService.CreateSkillAsync(skillRequestDto);
            return CreatedAtAction(nameof(GetSkillById), new { id = createdSkill.Id }, createdSkill);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] SkillRequestDto skillRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedSkill = await _skillService.UpdateSkillAsync(id, skillRequestDto);
            if (updatedSkill == null)
                return NotFound(new { message = $"Skill with ID {id} not found." });

            return Ok(updatedSkill);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var result = await _skillService.DeleteSkillAsync(id);
            if (!result)
                return NotFound(new { message = $"Skill with ID {id} not found." });

            return NoContent();
        }
    }
}