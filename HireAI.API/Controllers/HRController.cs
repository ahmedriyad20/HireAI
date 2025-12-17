using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Models.Identity;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR")]
    public class HRController : ControllerBase
    {
        private readonly IHRService _hrService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HRController(IHRService hrService, UserManager<ApplicationUser> userManager)
        {
            _hrService = hrService;
            _userManager = userManager;
        }

        //get hr details
        [HttpGet("{hrId:int}")]
        public async Task<IActionResult> GetHRAsync(int hrId)
        {
            var hr = await _hrService.GetHRAsync(hrId);
            if (hr == null)
                return NotFound();

            // Optional: Check if requesting user owns this profile
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (hr.Id != user.HRId)
                return Forbid();

            return Ok(hr);
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
