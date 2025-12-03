using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Applicant")]
    public class ApplicantDashboardController : ControllerBase
    {
        private readonly ApplicantDashboardService _applicantDashboardService;
        private readonly ApplicantApplicationService _applicantApplicationService;

        public ApplicantDashboardController(ApplicantDashboardService applicantDashboardService, ApplicantApplicationService applicantApplicationService)
        {
            _applicantDashboardService = applicantDashboardService;
            _applicantApplicationService = applicantApplicationService;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboardAsync(int Id)
        {
            int ActiveApplicationsNum = await _applicantDashboardService.GetActiveApplicationsNumberPerApplicantAsync(Id);
            int MockExamsTakenNumber = await _applicantDashboardService.GetMockExamsTakenNumberPerApplicantAsync(Id);
            double AverageExamsTakenScore = await _applicantDashboardService.GetAverageExamsTakenScorePerApplicantAsync(Id);
            string SkillLevel = await _applicantDashboardService.GetSkillLevelPerApplicantAsync(Id);
            var ApplicationTimeline = await _applicantDashboardService.GetApplicationTimelinePerApplicantAsync(Id);
            var ApplicantSkillImprovementScore = await _applicantDashboardService.GetApplicantSkillImprovementScoreAsync(Id);

            return Ok(new
            {
                ActiveApplicationsNum,
                MockExamsTakenNumber,
                AverageExamsTakenScore,
                SkillLevel,
                ApplicationTimeline,
                ApplicantSkillImprovementScore
            });
        }

        [HttpGet("{applicantId:int}/Applications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplicationsListAsync(int applicantId)
        {
            var applicationsList = await _applicantApplicationService.GetApplicantApplicationsList(applicantId);
            return Ok(applicationsList);
        }

        [HttpGet("{applicantId:int}/Applications/{applicationId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplicationDetailsAsync(int applicantId, int applicationId)
        {
            var applicationsDetails = await _applicantApplicationService.GetApplicationDetailsAsync(applicationId, applicantId);
            return Ok(applicationsDetails);
        }
    }
}
