using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JopController : ControllerBase
    {
        private readonly IJopOpenningService _jopOpenningService;
        public JopController(IJopOpenningService jopOpenningService)
        {

            _jopOpenningService = jopOpenningService;
        }
        [HttpPost]
        public async Task<IActionResult> AddJopOppenAsny([FromBody] Data.Helpers.DTOs.JopOpening.Request.JopOpeingRequestDto jopOpeingRequestDto)
        {
            await _jopOpenningService.CreateJopOppenAsny(jopOpeingRequestDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJopOppenAsny(int id)
        {
            await _jopOpenningService.DeleteJopOppenAsny(id);
            return Ok();
        }

        [HttpGet("hr/{hrid}")]
        public async Task<IActionResult> GetJopOpeningForHrAsync(int hrid)
        {
            var result = await _jopOpenningService.GetJopOpeningForHrAsync(hrid);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateJopOppenAsny(int id, [FromBody] Data.Helpers.DTOs.JopOpening.Request.JopOpeingRequestDto jopOpeingRequestDto)
        {
            await _jopOpenningService.UpdateJopOppenAsny(id, jopOpeingRequestDto);
            return Ok();
        }
    }
}
