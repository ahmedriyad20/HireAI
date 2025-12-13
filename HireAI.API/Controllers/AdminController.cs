using HireAI.Data.Helpers.DTOs.Applicant.Request;
using HireAI.Data.Helpers.DTOs.Application;
using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Helpers.DTOs.JobOpening.Request;
using HireAI.Data.Models.Identity;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = HireAI.Service.Interfaces.IAuthorizationService;

namespace HireAI.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IApplicantService _applicantService;
        private readonly IHRService _hrService;
        private readonly IApplicationService _applicationService;
        private readonly IJobPostService _jobPostService;

        public AdminController(IAuthorizationService authorizationService
            ,IApplicantService applicantService
            ,IHRService hrService
            ,IApplicationService applicationService
            ,IJobPostService jobPostService
            ,IGeminiService geminiService)
        {
            _authorizationService = authorizationService;
            _applicantService = applicantService;
            _hrService = hrService;
            _applicationService = applicationService;
            _jobPostService = jobPostService;
        }





        #region ApplicantControl


        [HttpGet("GetAllApplicants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllApplicantAsync()
        {
            var applicants = await _applicantService.GetAllApplicantsAsync();
            return Ok(applicants);
        }

        [HttpPut("ApplicantUpdate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplicantUpdateAsync(int id, [FromBody] ApplicantUpdateDto applicantDto)
        {
            if (id != applicantDto.Id)
                return BadRequest();


            var existingApplicant = await _applicantService.GetApplicantByIdAsync(id);
            if (existingApplicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            //if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
            //    return Forbid();

            var updatedApplicant = await _applicantService.UpdateApplicantAsync(applicantDto);
            return Ok(updatedApplicant);
        }

        [HttpDelete("ApplicantDelete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApplicantDeleteAsync(int id)
        {
            var applicant = await _applicantService.GetApplicantByIdAsync(id);
            if (applicant == null)
                return NotFound();

            // Check if the current applicant is the owner of the applicant data
            //if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, id))
            //    return Forbid();c

            await _applicantService.DeleteApplicantAsync(id);
            return NoContent();
        }
        #endregion



        #region HRControl


        [HttpGet("GetAllHR")]
        public async Task<IActionResult> GettAllHRAsync()
        {
            var hrs = await _hrService.GetAllHRAsync();
            return Ok(hrs);
        }
        [HttpPut("HRUpdate/{hrId:int}")]
        public async Task<IActionResult> UpdateHRAsync(int hrId, [FromBody] HRUpdateDto hrUpdateDto)
        {
            await _hrService.UpdateHRAsync(hrId, hrUpdateDto);
            return Ok();
        }
        [HttpDelete("HRDelete/{hrId:int}")]
        public async Task<IActionResult> DeleteHRAsync(int hrId)
        {
            await _hrService.DeleteHRAsync(hrId);

            return Ok();
        }
        #endregion

        #region ApplicationControl


        [HttpGet("GetAllApllications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return Ok(applications);
        }

        [HttpPut("ApplicationUpdate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplicationUpdateAsync(int id, [FromBody] UpdateApplicationDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var application = await _applicationService.UpdateApplicationAsync(updateDto);
                return Ok(application);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("ApplicationDelete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApplicationDeleteAsync(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);

            if (!result)
                return NotFound(new { message = $"Application with ID {id} not found" });

            return NoContent();
        }
        #endregion

        #region JobControl


        [HttpGet("GetAllJobOpenings")]
        public async Task<IActionResult> GetAllJobOpeningAsync()
        {
            var result = await _jobPostService.GetAllJobPostsAsync();
            return Ok(result);
        }
        [HttpDelete("JobOpeningDelete/{id}")]
        public async Task<IActionResult> DeleteJobOppenAsny(int id)
        {
            await _jobPostService.DeleteJobPostAsync(id);
            return Ok();
        }
        [HttpPut("JobOpeningUpdate/{id}")]
        public async Task<IActionResult> UpdateJobOppenAsny(int id, [FromBody] JobPostRequestDto JobOpeingRequestDto)
        {
            await _jobPostService.UpdateJobPostAsync(id, JobOpeingRequestDto);
            return Ok();
        }
        #endregion

    }
}
