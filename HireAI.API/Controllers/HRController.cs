using HireAI.Service.Implementation;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly IHRService _IHRService;

        public HRController(IHRService hrDashboardService)
        {
            _IHRService = hrDashboardService;
        }

        [HttpGet("dashboard/{hrId:int}")]
        public async Task<IActionResult> GetHRDashBoardAsync(int hrId)
        {
            var dashboard = await _IHRService.GetDashboardAsync(hrId);
            return Ok(dashboard);
        }

        [HttpGet("{hrId:int}")]
        public async  Task<IActionResult> GetHRAsync(int hrId)
        {
            var hr = await _IHRService.GetHRAsync(hrId);
            return Ok(hr);
        }
    }
}
