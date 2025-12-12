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
    
    public class ApplicantController : ControllerBase
    {

        private readonly ApplicantDashboardService _applicantDashboardService;
        private readonly ApplicantApplicationService _applicantApplicationService;
        private readonly IS3Service _s3Service;
        private readonly Service.Interfaces.IAuthorizationService _authorizationService;
        private readonly IApplicantService _applicantService;

        public ApplicantController(Service.Interfaces.IAuthorizationService authorizationService,
            IApplicantService applicantService, ApplicantDashboardService applicantDashboardService, ApplicantApplicationService applicantApplicationService, IS3Service s3Service, UserManager<ApplicationUser> userManager)
        {
            _applicantDashboardService = applicantDashboardService;
            _applicantApplicationService = applicantApplicationService;
            _s3Service = s3Service;
            _authorizationService = authorizationService;
            _applicantService = applicantService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> GetAllAsync()
        {
            var applicants = await _applicantService.GetAllApplicantsAsync();
            return Ok(applicants);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Applicant")]
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
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> CreateAsync([FromBody] Applicant applicant)
        {
            var createdApplicant = await _applicantService.AddApplicantAsync(applicant);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdApplicant.Id }, createdApplicant);
        }*/

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] ApplicantUpdateDto applicantDto)
        {
            if (id != applicantDto.Id)
                return BadRequest();
            

            var existingApplicant = await _applicantService.GetApplicantByIdAsync(id);
            if (existingApplicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
                return Forbid();

            var updatedApplicant = await _applicantService.UpdateApplicantAsync(applicantDto);
            return Ok(updatedApplicant);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Applicant")]
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

        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [Route("UploadResume/{applicantId:int}")]
        [Authorize(Roles = "Applicant")]
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

        /// <summary>
        /// Download a resume file from S3 by file key
        /// Usage: GET /api/applicant/DownloadResume?fileKey=cv/1139f78b-58d4-40ae-ae91-9a1d8d897c7f_ahmed_Egndy_cv%20(8).pdf
        /// </summary>
        [HttpGet("DownloadResume")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Applicant,HR")]
        public async Task<IActionResult> DownloadResumeAsync([FromQuery] string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest(new { error = "File key cannot be empty." });
            }

            try
            {
                // Download file from S3 using the file key
                var downloadDto = await _s3Service.DownloadFileAsync(fileKey);
                
                // Return file as download with appropriate content type
                return File(downloadDto.FileStream, downloadDto.ContentType ?? "application/octet-stream", downloadDto.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { error = "File not found in S3." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to download file: {ex.Message}" });
            }
        }

        /// <summary>
        /// Download a resume file from S3 using the full S3 URL
        /// Usage: GET /api/applicant/DownloadResumeByUrl?s3Url=https://hireaibucket.s3.us-east-1.amazonaws.com/cv/1139f78b-58d4-40ae-ae91-9a1d8d897c7f_ahmed_Egndy_cv%20(8).pdf
        /// </summary>
        [HttpGet("DownloadResumeByUrl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Applicant,HR")]
        public async Task<IActionResult> DownloadResumeByUrlAsync([FromQuery] string s3Url)
        {
            if (string.IsNullOrWhiteSpace(s3Url))
            {
                return BadRequest(new { error = "S3 URL cannot be empty." });
            }

            try
            {
                // Extract file key from the S3 URL
                var fileKey = ExtractFileKeyFromUrl(s3Url);

                if (string.IsNullOrWhiteSpace(fileKey))
                {
                    return BadRequest(new { error = "Invalid S3 URL format." });
                }

                // Download file from S3
                var downloadDto = await _s3Service.DownloadFileAsync(fileKey);
                
                // Return file as download
                return File(downloadDto.FileStream, downloadDto.ContentType ?? "application/octet-stream", downloadDto.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { error = "File not found in S3." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to download file: {ex.Message}" });
            }
        }

        /// <summary>
        /// Extracts the file key from an S3 URL
        /// Example Input: https://hireaibucket.s3.us-east-1.amazonaws.com/cv/1139f78b-58d4-40ae-ae91-9a1d8d897c7f_ahmed_Egndy_cv%20(8).pdf
        /// Example Output: cv/1139f78b-58d4-40ae-ae91-9a1d8d897c7f_ahmed_Egndy_cv%20(8).pdf
        /// </summary>
        private string ExtractFileKeyFromUrl(string s3Url)
        {
            try
            {
                var uri = new Uri(s3Url);
                var fileKey = uri.AbsolutePath.TrimStart('/');
                return fileKey;
            }
            catch
            {
                return null;
            }
        }
    }
}
