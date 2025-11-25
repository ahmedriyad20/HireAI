using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HireAI.API.Controllers
{
[Route("api/[controller]")]
    [ApiController]
    public class AvailableJobsController : ControllerBase
    {
        private readonly IApplicantJobOpeningService _applicantJobOpeningService;
        public AvailableJobsController(IApplicantJobOpeningService applicantJobOpeningService)
        {
            _applicantJobOpeningService = applicantJobOpeningService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobOpenings()
        {

            var JobOpenings = await _applicantJobOpeningService.GetAllJobOpeningAsync();
            // Placeholder implementation
            return Ok(JobOpenings);
        }
    }
}
