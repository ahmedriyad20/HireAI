using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR,Applicant")]
    public class JopController : ControllerBase
    {
        private readonly IJobPostService _jopPostService;
        public JopController(IJobPostService jobPostService)
        {

            _jopPostService = jobPostService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJopOppenAsny(int id)
        {
            var result = await _jopPostService.GetJobPostAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddJopOppenAsny([FromBody] JobPostRequestDto jopOpeingRequestDto)
        {
            await _jopPostService.CreateJobPostAsync(jopOpeingRequestDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJopOppenAsny(int id)
        {
            await _jopPostService.DeleteJobPostAsync(id);
            return Ok();
        }

        [HttpGet("hr/{hrid}")]
        public async Task<IActionResult> GetJopOpeningForHrAsync(int hrid)
        {
            var result = await _jopPostService.GetJobPostForHrAsync(hrid);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateJopOppenAsny(int id, [FromBody] Data.Helpers.DTOs.JopOpening.Request.JobPostRequestDto jopOpeingRequestDto)
        {
            await _jopPostService.UpdateJobPostAsync(id, jopOpeingRequestDto);
            return Ok();
        }
    }
}
