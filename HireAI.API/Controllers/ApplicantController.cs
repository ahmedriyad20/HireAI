using HireAI.Data.Models;
using HireAI.Data.Models.Identity;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Applicant")]
    public class ApplicantController : ControllerBase
    {
        private readonly Service.Interfaces.IAuthorizationService _authorizationService;
        private readonly IApplicantService _applicantService;

        public ApplicantController(Service.Interfaces.IAuthorizationService authorizationService, 
            IApplicantService applicantService)
        {
            _authorizationService = authorizationService;
            _applicantService = applicantService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var applicants = await _applicantService.GetAllApplicantsAsync();
            return Ok(applicants);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            /*// Get applicantId from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _userManager.FindByIdAsync(userIdClaim);

            // You need to get ApplicantId from ApplicationUser
            // For now, assuming you pass it in the request or retrieve it
            // Better: Add ApplicantId as a custom claim in JWT
            int applicantId = user.ApplicantId ?? 0;
            var applicant = await _applicantService.GetApplicantByIdAsync(applicantId);
            if (applicant == null)
                return NotFound();

            return Ok(applicant);*/

            var applicant = await _applicantService.GetApplicantByIdAsync(id);
            if (applicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
                return Forbid();

            return Ok(applicant);
        }

        /*[HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] Applicant applicant)
        {
            var createdApplicant = await _applicantService.AddApplicantAsync(applicant);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdApplicant.Id }, createdApplicant);
        }*/

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Applicant applicant)
        {
            if (id != applicant.Id)
                return BadRequest();
            

            var existingApplicant = await _applicantService.GetApplicantByIdAsync(id);
            if (existingApplicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
                return Forbid();

            var updatedApplicant = await _applicantService.UpdateApplicantAsync(applicant);
            return Ok(updatedApplicant);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var applicant = await _applicantService.GetApplicantByIdAsync(id);
            if (applicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
                return Forbid();

            await _applicantService.DeleteApplicantAsync(id);
            return NoContent();
        }
    }
}
