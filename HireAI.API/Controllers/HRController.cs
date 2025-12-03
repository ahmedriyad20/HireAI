using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Service.Services;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly IHRService _hrService;

        public HRController(IHRService hrService)
        {
            _hrService = hrService;
        }

        //get hr details
        [HttpGet("{hrId:int}")]
        public async Task<IActionResult> GetHRAsync(int hrId)
        {
            return Ok(await _hrService.GetHRAsync(hrId));
        }
        [HttpPut("{hrId:int}")]
        public async Task<IActionResult> UpdateHRAsync(int hrId,[FromBody] HRUpdateDto hrUpdateDto)
        {
            await _hrService.UpdateHRAsync(hrId, hrUpdateDto);
            return Ok();
        }
        [HttpDelete("{hrId:int}")]
        public async Task<IActionResult> DeleteHRAsync(int hrId)
        {
            await _hrService.DeleteHRAsync(hrId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHRAsync(HRCreateDto hrCreateDto)
        {
            await _hrService.CreateHRAsync(hrCreateDto);
            return Ok();
        }   
    }

}
