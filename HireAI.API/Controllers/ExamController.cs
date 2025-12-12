using HireAI.Data.Helpers.DTOs.Exam.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Models;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
        private readonly IApplicationService _applicationService;

        public ExamController(MockExamService mockExamService, IExamService examService, Service.Interfaces.IAuthorizationService authorizationService,
            IApplicationService applicationService)
        {
            _mockExamService = mockExamService;
            _examService = examService;
            _authorizationService = authorizationService;
            _applicationService = applicationService;
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

        [HttpGet("id/{examId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamById(int examId)
        {
            var examDTO = await _examService.GetExamByIdAsync(examId);
            if (examDTO == null)
                return NotFound(new { error = "Exam not found" });

            return Ok(examDTO);
        }



        //Gendy Exam Controller Methods
        [HttpGet("{applicantId:int}")]
        public async Task<IActionResult> GetExamByApplicantId(int applicantId)
        {

           Console.WriteLine("Received request for applicant ID: " + applicantId);
            var examDTO =  await _examService.GetExamByApplicantIdAsync(applicantId);
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

        /// <summary>
        /// Creates a job exam with AI-generated questions based on the job description
        /// </summary>
        /// <param name="applicationId">The application ID to create the exam for</param>
        /// <returns>List of generated questions</returns>
        [HttpPost("JobExamByAI/{applicationId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateJobExamAsync(int applicationId)
        {
            try
            {
                

                var questions = await _examService.CreateJobExamAsync(applicationId);

                var application = await _applicationService.GetApplicationByIdAsync(applicationId);
                if (application == null)
                    throw new Exception("Application not found");

                var examDto = await _examService.GetExamByIdAsync(application.ExamId ?? 0);

                return Ok(new
                {
                    message = "Job exam created successfully with AI-generated questions",
                    questionCount = questions.Count,
                    ExamDurationInMinutes = examDto?.DurationInMinutes,
                    Questions = questions
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a mock exam with AI-generated questions based on the exam description
        /// </summary>
        /// <param name="examId">The exam ID to generate questions for</param>
        /// <returns>List of generated questions</returns>
        [HttpPost("MockExamByAI/{examId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateMockExamAsync(int examId)
        {
            try
            {
                var questions = await _examService.CreateMockExamAsync(examId);

                var examDto = await _examService.GetExamByIdAsync(examId);

                return Ok(new
                {
                    message = "Mock exam questions generated successfully",
                    questionCount = questions.Count,
                    ExamDurationInMinutes = examDto?.DurationInMinutes,
                    Questions = questions
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Evaluates an exam and creates ExamSummary and ExamEvaluation records
        /// </summary>
        /// <param name="evaluationRequest">The exam evaluation data</param>
        /// <returns>The created ExamSummary record</returns>
        [HttpPost("evaluate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EvaluateExamAsync([FromBody] ExamEvaluationRequestDTO evaluationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var examSummary = new ExamSummary();
                if (evaluationRequest.ApplicantId.HasValue) // If the frontend provides an ApplicantId, it's a mock exam
                {
                    examSummary = await _examService.EvaluateMockExamAsync(evaluationRequest);
                }
                else
                {
                    examSummary = await _examService.EvaluateJobExamAsync(evaluationRequest);
                }
                   

                return Created("",
                    new
                    {
                        message = "Exam evaluated successfully",
                        evaluationId = examSummary.ExamEvaluationId,
                        examSummaryId = examSummary.Id,
                        examId = examSummary.ExamId,
                        applicantExamScore = examSummary.ApplicantExamScore
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
