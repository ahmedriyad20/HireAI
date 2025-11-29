using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JopOpenningController : ControllerBase
    {
        private readonly IJopOpenningService _jopOpenningService;
        public JopOpenningController(IJopOpenningService jopOpenningService)
        {
            _jopOpenningService = jopOpenningService;
        }
        [HttpPost]
        public IActionResult CreateJobOpening(CreateJopOpeingRequestDto jopOpeingRequestDto)
        {
             _jopOpenningService.AddJopOppenAsny(jopOpeingRequestDto);

            // Return 201 with the location of the new resource
            return Ok("Job opening created successfully.");
        }
    }
}
