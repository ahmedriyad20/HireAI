using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR")]
    public class HRDashboardController:ControllerBase
    {


        private readonly IHrDashboardService _hrDashboardService;

        public HRDashboardController(IHrDashboardService dashboardService)
        {
            _hrDashboardService = dashboardService;
        }

        [HttpGet("{hrId:int}")]   
        public async Task<IActionResult> GetDashboard(int hrId)
        {
            var dashboardDto = await _hrDashboardService.GetDashboardAsync(hrId);
            return Ok(dashboardDto);
        }   
        // -------------------------------
        // Individual metric endpoints
        // -------------------------------

        [HttpGet("{hrId:int}/total-applicants")]
        public async Task<IActionResult> GetTotalApplicants(int hrId)
            => Ok(await _hrDashboardService.GetTotalApplicantsAsync(hrId));

        [HttpGet("{hrId:int}/exam-taken")]
        public async Task<IActionResult> GetTotalExamTaken(int hrId)
            => Ok(await _hrDashboardService.GetTotalExamTakenAsync(hrId));

        [HttpGet("{hrId:int}/top-candidates")]
        public async Task<IActionResult> GetTotalTopCandidates(int hrId)
            => Ok(await _hrDashboardService.GetTotalTopCandidatesAsync(hrId));

        [HttpGet("{hrId:int}/monthly-applicants")]
        public async Task<IActionResult> GetMonthlyApplicants(int hrId)
            => Ok(await _hrDashboardService.GetMonthlyNumberOfApplicationsAsync(hrId));

        [HttpGet("{hrId:int}/ats-passed-monthly")]
        public async Task<IActionResult> GetMonthlyATSPassed(int hrId)
            => Ok(await _hrDashboardService.GetMonthlyOfTotalATSPassedAsync(hrId));

        [HttpGet("{hrId:int}/recent-applications")]
        public async Task<IActionResult> GetRecentApplications(int hrId)
            => Ok(await _hrDashboardService.GetRecentApplicantsAsync(hrId));

        [HttpGet("{hrId:int}/active-jobs")]
        public async Task<IActionResult> GetActiveJobs(int hrId)
            => Ok(await _hrDashboardService.GetActiveJobPostingsAsync(hrId));
    }
}

