using HireAI.Service.Services;
using Microsoft.AspNetCore.Http;

﻿using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR,Applicant")]
    public class ExamController : ControllerBase
    {
        private readonly MockExamService _mockExamService;
        private readonly IExamService _examService;
        private readonly Service.Interfaces.IAuthorizationService _authorizationService;

        public ExamController(MockExamService mockExamService, IExamService examService, Service.Interfaces.IAuthorizationService authorizationService)
        {
            _mockExamService = mockExamService;
            _examService = examService;
            _authorizationService = authorizationService;
        }

        //Riyad Mock Exam Controller Methods
        [HttpGet("QuickStats/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMockExamQuickStatsAsync(int applicantId)
        {
            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, applicantId))
                return Forbid();

            int MockExamsTakenNumber = await _mockExamService.GetMockExamsTakenNumberPerApplicantAsync(applicantId);
            int MockExamsTakenNumberForCurrentMonth = await _mockExamService.GetMockExamsTakenNumberForCurrentMonthPerApplicantAsync(applicantId);
            double AverageExamsTakenScore = await _mockExamService.GetAverageExamsTakenScorePerApplicantAsync(applicantId);
            double AverageExamsTakenScoreImprovement = await _mockExamService.GetAverageExamsTakenScoreImprovementPerApplicantAsync(applicantId);
            return Ok(new
            {
                MockExamsTakenNumber,
                MockExamsTakenNumberForCurrentMonth,
                AverageExamsTakenScore,
                AverageExamsTakenScoreImprovement
            });
        }

        [HttpGet("RecommendedMockExams/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendedMockExamsAsync(int applicantId)
        {
            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, applicantId))
                return Forbid();

            var recommendedMockExams = await _mockExamService.GetRecommendedMockExamsPerApplicantAsync(applicantId);
            return Ok(recommendedMockExams);
        }

        [HttpGet("AllMockExams/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMockExamsAsync(int applicantId)
        {
            // Check if the current applicant is the owner of the applicant data
            if (!await _authorizationService.ValidateApplicantOwnershipAsync(User, applicantId))
                return Forbid();

            var allMockExams = await _mockExamService.GetAllMockExamsPerApplicantAsync(applicantId);
            return Ok(allMockExams);
        }



        //Gendy Exam Controller Methods
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExamByApplicantId(int id)
        {

           Console.WriteLine("Received request for applicant ID: " + id);
            var examDTO =  await _examService.GetExamByApplicantIdAsync(id);
          return Ok(examDTO);

        }
        [HttpGet("taken/{applicantId:int}")]
        public async Task<IActionResult> GetExamsTakenByApplicant(int applicantId, int pageNumber = 1, int pageSize = 5)
        {
            var examsDTO = await _examService.GetExamsTakenByApplicant(applicantId, pageNumber, pageSize);
            return Ok(examsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam([FromBody] ExamRequestDTO examRequestDTO)
        {
            await _examService.CreateExamAsync(examRequestDTO);
            return Ok();
        }

        [HttpPost("question")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequestDTO questionRequestDTO)
        {
            await _examService.CreateQuestionAsync(questionRequestDTO);
            return Ok();
        }
        [HttpDelete("{examId:int}")]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            await _examService.DeleteExamAsync(examId);
            return Ok();
        }
    }
}
