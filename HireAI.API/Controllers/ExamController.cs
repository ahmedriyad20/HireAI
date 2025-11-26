using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService) { 
            _examService = examService;
        }

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
    }
}
