using HireAI.Data.Helpers.DTOs.Application;
using HireAI.Data.Helpers.Enums;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IJobPostService _jobPostService;
        private readonly IS3Service _s3Service;
        private readonly IGeminiService _geminiService;

        public ApplicationController(
            IApplicationService applicationService, 
            IJobPostService jobPostService,
            IS3Service s3Service,
            IGeminiService geminiService)
        {
            _applicationService = applicationService;
            _jobPostService = jobPostService;
            _s3Service = s3Service;
            _geminiService = geminiService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return Ok(applications);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            
            if (application == null)
                return NotFound(new { message = $"Application with ID {id} not found" });

            return Ok(application);
        }

        [HttpGet("applicant/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> GetByApplicantIdAsync(int applicantId)
        {
            var applications = await _applicationService.GetApplicationsByApplicantIdAsync(applicantId);
            return Ok(applications);
        }

        [HttpGet("job/{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByJobIdAsync(int jobId)
        {
            var applications = await _applicationService.GetApplicationsByJobIdAsync(jobId);
            return Ok(applications);
        }

        [HttpGet("hr/{hrId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetByHRIdAsync(int hrId)
        {
            var applications = await _applicationService.GetApplicationsByHRIdAsync(hrId);
            return Ok(applications);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateApplicationDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var application = await _applicationService.CreateApplicationAsync(createDto);
                return Ok(application);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateApplicationDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var application = await _applicationService.UpdateApplicationAsync(updateDto);
                return Ok(application);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "HR,Applicant")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);
            
            if (!result)
                return NotFound(new { message = $"Application with ID {id} not found" });

            return NoContent();
        }

        [HttpGet("analyze/{applicationID}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AnalyzeCV(int applicationID)
        {
            var application = await _applicationService.GetApplicationByIdAsync(applicationID);

            if (application == null)
                return NotFound(new { message = $"Application with ID {applicationID} not found" });

            // Validate CV file path exists
            if (string.IsNullOrWhiteSpace(application.CVFilePath))
                return BadRequest(new { message = "Application does not have a CV file attached" });

            var AppliedJob = await _jobPostService.GetJobPostAsync(application.JobId ?? 0);

            if (AppliedJob == null)
                return NotFound(new { message = $"Job Post with ID {application.JobId} not found" });

            if (string.IsNullOrWhiteSpace(AppliedJob.Description))
                return BadRequest(new { message = "Job description is missing" });

            try
            {
                // Download CV from S3
                var cvFile = await _s3Service.DownloadFileToMemoryAsync(application.CVFilePath);

                // Analyze CV using Gemini
                var analysisResult = await _geminiService.AnalyzeCVAsync(
                    cvFile.FileContent, 
                    AppliedJob.Description, 
                    cvFile.FileName);

                    // Update application with analysis results
                    var updateDto = new UpdateApplicationDto
                    {
                        Id = applicationID,
                        ApplicationStatus = analysisResult.RecommendedStatus,
                        AtsScore = analysisResult.AtsScore,
                        CVFilePath = application.CVFilePath,
                        ExamStatus = enExamStatus.NotTaken
                    };

                await _applicationService.UpdateApplicationAsync(updateDto);

                return Ok(new
                {
                    applicationId = applicationID,
                    atsScore = analysisResult.AtsScore,
                    status = analysisResult.RecommendedStatus.ToString(),
                    feedback = analysisResult.Feedback,
                    skillsFound = analysisResult.SkillsFound,
                    skillsGaps = analysisResult.SkillsGaps,
                    message = "CV analysis completed successfully"
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { message = "AI service temporarily unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Failed to analyze CV", details = ex.Message });
            }
        }
    }
}