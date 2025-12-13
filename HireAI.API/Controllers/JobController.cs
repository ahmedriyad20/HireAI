using HireAI.Data.Helpers.DTOs.JobOpening.Request;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR,Applicant")]
    public class JobController : ControllerBase
    {
        private readonly IJobPostService _JobPostService;
        public JobController(IJobPostService jobPostService)
        {

            _JobPostService = jobPostService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobOppenAsny(int id)
        {
            var result = await _JobPostService.GetJobPostAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddJobOppenAsny([FromBody] JobPostRequestDto JobOpeingRequestDto)
        {
            await _JobPostService.CreateJobPostAsync(JobOpeingRequestDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobOppenAsny(int id)
        {
            await _JobPostService.DeleteJobPostAsync(id);
            return Ok();
        }

        [HttpGet("hr/{hrid}")]
        public async Task<IActionResult> GetJobOpeningForHrAsync(int hrid)
        {
            var result = await _JobPostService.GetJobPostForHrAsync(hrid);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateJobOppenAsny(int id, [FromBody] JobPostRequestDto JobOpeingRequestDto)
        {
            await _JobPostService.UpdateJobPostAsync(id, JobOpeingRequestDto);
            return Ok();
        }

        [HttpGet("{jobId}/applications-count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalApplicationsByJobIdAsync(int jobId)
        {
            var count = await _JobPostService.GetTotalApplicationsByJobIdAsync(jobId);
            return Ok(new { jobId, totalApplications = count });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobOpeningAsync()
        {
            var result = await _JobPostService.GetAllJobPostsAsync();
                return Ok(result);
            
        }
    }
}
