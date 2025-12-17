
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IReportPdfService _pdfService;

        public ReportController(IReportService reportService, IReportPdfService pdfService)
        {
            _reportService = reportService;
            _pdfService = pdfService;
        }

        // GET /api/report/{jobId}
        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetReport(int jobId)
        {
            var report = await _reportService.GetReportByJobIdAsync(jobId);
            if (report == null)
                return NotFound(new { message = "Job not found" });

            return Ok(report);
        }

        // GET /api/report/pdf/{jobId}  --> Download PDF
        [HttpGet("pdf/{jobId}")]
        public async Task<IActionResult> GetReportPdf(int jobId)
        {
            var report = await _reportService.GetReportByJobIdAsync(jobId);
            if (report == null)
                return NotFound(new { message = "Job not found" });

            var pdfBytes = _pdfService.GeneratePdf(report);

            return File(pdfBytes, "application/pdf", $"JobReport_{report.JobTitle}.pdf");
        }
    }
}
