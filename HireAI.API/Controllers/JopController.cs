using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JopController : ControllerBase
    {
        private readonly IJopService _jopPostService;
        public JopController(IJopService jobPostService)
        {

            _jopPostService = jobPostService;
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
