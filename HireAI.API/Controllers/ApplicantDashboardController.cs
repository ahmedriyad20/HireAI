using HireAI.Data.Models.Identity;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Applicant")]
    public class ApplicantDashboardController : ControllerBase
    {
        private readonly Service.Interfaces.IAuthorizationService _authorizationService;
        private readonly ApplicantDashboardService _applicantDashboardService;
        private readonly ApplicantApplicationService _applicantApplicationService;

        public ApplicantDashboardController(Service.Interfaces.IAuthorizationService authorizationService, ApplicantDashboardService applicantDashboardService, 
            ApplicantApplicationService applicantApplicationService)
        {
            _authorizationService = authorizationService;
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

            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, Id))
                return Forbid();

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
            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, applicantId))
                return Forbid();

            var applicationsList = await _applicantApplicationService.GetApplicantApplicationsList(applicantId);
            return Ok(applicationsList);
        }

        [HttpGet("{applicantId:int}/Applications/{applicationId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApplicationDetailsAsync(int applicantId, int applicationId)
        {
            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, applicantId))
                return Forbid();

            var applicationsDetails = await _applicantApplicationService.GetApplicationDetailsAsync(applicationId, applicantId);
            return Ok(applicationsDetails);
        }
    }
}
