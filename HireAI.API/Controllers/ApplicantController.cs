using HireAI.Data.Helpers.DTOs.Applicant.Request;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Service.Interfaces;
using Humanizer;
using HireAI.Data.Models.Identity;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Applicant")]
    public class ApplicantController : ControllerBase
    {

        private readonly ApplicantDashboardService _applicantDashboardService;
        private readonly ApplicantApplicationService _applicantApplicationService;
        private readonly IS3Service _s3Service;
        private readonly IApplicantService _applicantService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicantController(IApplicantService applicantService, ApplicantDashboardService applicantDashboardService, ApplicantApplicationService applicantApplicationService, IS3Service s3Service, UserManager<ApplicationUser> userManager)
        {
            _applicantDashboardService = applicantDashboardService;
            _applicantApplicationService = applicantApplicationService;
            _s3Service = s3Service;
            _applicantService = applicantService;
            _userManager = userManager;
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

            // Optional: Check if requesting user owns this profile
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (applicant.Id != user.ApplicantId)
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

            await _applicantService.DeleteApplicantAsync(id);
            return NoContent();
        }

        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [Route("UploadResume/{applicantId:int}")]
        public async Task<IActionResult> UploadResumeAsync([FromForm] ApplicantCreateDto dto)
        {
            if (dto == null || dto.CvFile == null)
            {
                return BadRequest("No file uploaded.");
            }

            // Upload to S3
            string resumeUrl;
            try
            {
                // Cast dto.CvFile to Microsoft.AspNetCore.Http.IFormFile f possible
                resumeUrl = await _s3Service.UploadFileAsync(dto.CvFile );
            }
            catch (Exception ex)
            {
                // log ex in real app
                return StatusCode(500, $"Failed to upload file: {ex.Message}");
            }
       

            // The rest of your logic here...
            // return Ok(resumeUrl); // or whatever is appropriate
            return Ok(resumeUrl);
        }
    }
}
