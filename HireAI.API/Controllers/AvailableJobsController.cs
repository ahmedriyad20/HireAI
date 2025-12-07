using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR,Applicant")]
    public class AvailableJobsController : ControllerBase
    {
        private readonly IApplicantJobPostService _applicantJobOpeningService;
        public AvailableJobsController(IApplicantJobPostService applicantJobOpeningService)
        {
            _applicantJobOpeningService = applicantJobOpeningService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllJobOpenings()
        {

            var JobOpenings = await _applicantJobOpeningService.GetAllJobPostAsync();
            // Placeholder implementation
            return Ok(JobOpenings);
        }
    }
}
