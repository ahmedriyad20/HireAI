using HireAI.Service.Implementation;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HRDashBoardController : ControllerBase
    {
        private readonly IHrDashboardService _IHrDashboardService;

        public HRDashBoardController(IHrDashboardService hrDashboardService)
        {
            _IHrDashboardService = hrDashboardService;
        }

        [HttpGet("{hrId:int}")]
        public async Task<IActionResult> GetHRDashBoardAsync(int hrId)
        {
            var dashboard = await _IHrDashboardService.GetDashboardAsync(hrId);
            return Ok(dashboard);
        }   
    }
}
