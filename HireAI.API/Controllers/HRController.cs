using HireAI.Service.Implementation;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly IHRService _hrService;

        public HRController(IHRService hrService)
        {
            _hrService = hrService;
        }

        [HttpGet("dashboard/{hrId:int}")]
        public async Task<IActionResult> GetHRDashBoardAsync(int hrId)
        {
            return Ok(await _hrService.GetDashboardAsync(hrId));
        }

        [HttpGet("{hrId:int}")]
        public async Task<IActionResult> GetHRAsync(int hrId)
        {
            return Ok(await _hrService.GetHRAsync(hrId));
        }

        // -------------------------------
        // Individual metric endpoints
        // -------------------------------

        [HttpGet("{hrId:int}/total-applicants")]
        public async Task<IActionResult> GetTotalApplicants(int hrId)
            => Ok(await _hrService.GetTotalApplicantsAsync(hrId));

        [HttpGet("{hrId:int}/exam-taken")]
        public async Task<IActionResult> GetTotalExamTaken(int hrId)
            => Ok(await _hrService.GetTotalExamTakenAsync(hrId));

        [HttpGet("{hrId:int}/top-candidates")]
        public async Task<IActionResult> GetTotalTopCandidates(int hrId)
            => Ok(await _hrService.GetTotalTopCandidatesAsync(hrId));

        [HttpGet("{hrId:int}/monthly-applicants")]
        public async Task<IActionResult> GetMonthlyApplicants(int hrId)
            => Ok(await _hrService.GetMonthlyNumberOfApplicationsAsync(hrId));

        [HttpGet("{hrId:int}/ats-passed-monthly")]
        public async Task<IActionResult> GetMonthlyATSPassed(int hrId)
            => Ok(await _hrService.GetMonthlyOfTotalATSPassedAsync(hrId));

        [HttpGet("{hrId:int}/recent-applications")]
        public async Task<IActionResult> GetRecentApplications(int hrId)
            => Ok(await _hrService.GetRecentApplicantsAsync(hrId));

        [HttpGet("{hrId:int}/active-jobs")]
        public async Task<IActionResult> GetActiveJobs(int hrId)
            => Ok(await _hrService.GetActiveJobPostingsAsync(hrId));
    }

}
