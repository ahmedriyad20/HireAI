using HireAI.Service.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantDashboardController : ControllerBase
    {
        private readonly ApplicantDashboardService _applicantDashboardService;

        public ApplicantDashboardController(ApplicantDashboardService applicantDashboardService)
        {
            _applicantDashboardService = applicantDashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicantDashboardAsync(int applicantId)
        {
            int ActiveApplicationsNum = await _applicantDashboardService.GetActiveApplicationsNumberPerApplicantAsync(applicantId);
            int MockExamsTakenNumber = await _applicantDashboardService.GetMockExamsTakenNumberPerApplicantAsync(applicantId);
            double AverageExamsTakenScore = await _applicantDashboardService.GetAverageExamsTakenScorePerApplicantAsync(applicantId);
            string SkillLevel = await _applicantDashboardService.GetSkillLevelPerApplicantAsync(applicantId);
            var ApplicationTimeline = await _applicantDashboardService.GetApplicationTimelinePerApplicantAsync(applicantId);
            var ApplicantSkillImprovementScore = await _applicantDashboardService.GetApplicantSkillImprovementScoreAsync(applicantId);

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
    }
}
