using HireAI.Data.Helpers.DTOs;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "HR")]
    public class JobDetailsController : ControllerBase
    {
        private readonly IJobDetailsService _jobDetailsService;

        public JobDetailsController(IJobDetailsService jobDetailsService)
        {
            _jobDetailsService = jobDetailsService;
        }
                                    
        /// <summary>
        /// Get all jobs for a specific HR
        /// </summary>
        /// <param name="hrid">The ID of the HR</param>
        /// <returns>List of job summaries with statistics</returns>
        [HttpGet("AllJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentHRJobsAsync(int hrid)
        {
            try
            {
                var jobs = await _jobDetailsService.GetAllJobsForHRAsync(hrid);
                return Ok(new
                {
                    success = true,
                    hrId = hrid,
                    message = $"Retrieved {jobs.Count} jobs for HR {hrid}",
                    data = jobs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving jobs: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get detailed information about a specific job including all applications and top applicants
        /// </summary>
        /// <param name="jobId">The ID of the job</param>
        /// <returns>Detailed job information with applications and rankings</returns>
        [HttpGet("{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobDetailsAsync(int jobId)
        {
            try
            {
                // Get HR ID from JWT token
                var hrIdClaim = User.FindFirst("HRId")?.Value;
                
                if (!int.TryParse(hrIdClaim, out int hrId))
                {
                    return Unauthorized(new { error = "HR ID not found in token" });
                }

                var jobDetails = await _jobDetailsService.GetJobDetailsAsync(jobId, hrId);
                return Ok(new
                {
                    success = true,
                    message = "Job details retrieved successfully",
                    data = jobDetails
                });     
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving job details: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get all applications for a specific job
        /// </summary>
        /// <param name="jobId">The ID of the job</param>
        /// <returns>List of all applications for the job</returns>
        [HttpGet("{jobId}/Applications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobApplicationsAsync(int jobId)
        {
            try
            {
                var hrIdClaim = User.FindFirst("HRId")?.Value;
                
                if (!int.TryParse(hrIdClaim, out int hrId))
                {
                    return Unauthorized(new { error = "HR ID not found in token" });
                }

                var applications = await _jobDetailsService.GetJobApplicationsAsync(jobId, hrId);
                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {applications.Count} applications",
                    data = applications
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving applications: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get top applicants for a specific job (ranked by ATS score)
        /// </summary>
        /// <param name="jobId">The ID of the job</param>
        /// <param name="topCount">Number of top applicants to retrieve (default: 10)</param>
        /// <returns>List of top applicants ranked by ATS score</returns>
        [HttpGet("{jobId}/TopApplicants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopApplicantsAsync(int jobId, [FromQuery] int topCount = 10)
        {
            try
            {
                var hrIdClaim = User.FindFirst("HRId")?.Value;
                
                if (!int.TryParse(hrIdClaim, out int hrId))
                {
                    return Unauthorized(new { error = "HR ID not found in token" });
                }

                var topApplicants = await _jobDetailsService.GetTopApplicantsAsync(jobId, hrId, topCount);
                return Ok(new
                {
                    success = true,
                    message = $"Retrieved top {topApplicants.Count} applicants",
                    data = topApplicants
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving top applicants: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get top exam takers for a specific job (ranked by exam score)
        /// </summary>
        /// <param name="jobId">The ID of the job</param>
        /// <param name="topCount">Number of top exam takers to retrieve (default: 10)</param>
        /// <returns>List of top exam takers ranked by exam score</returns>
        [HttpGet("{jobId}/TopExamTakers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopExamTakersAsync(int jobId, [FromQuery] int topCount = 10)
        {
            try
            {
                var hrIdClaim = User.FindFirst("HRId")?.Value;
                
                if (!int.TryParse(hrIdClaim, out int hrId))
                {
                    return Unauthorized(new { error = "HR ID not found in token" });
                }

                var topExamTakers = await _jobDetailsService.GetTopExamTakersAsync(jobId, hrId, topCount);
                return Ok(new
                {
                    success = true,
                    message = $"Retrieved top {topExamTakers.Count} exam takers",
                    data = topExamTakers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving top exam takers: {ex.Message}" });
            }
        }
    }
}
