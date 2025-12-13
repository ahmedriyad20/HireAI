using HireAI.Data.Models.Identity;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ValidateApplicantOwnershipAsync(ClaimsPrincipal user, int applicantId)
        {
            // Skip ownership validation if user is admin
            if (IsAdmin(user))
                return true;

            var currentUser = await GetCurrentUserAsync(user);
            return currentUser?.ApplicantId == applicantId;
        }

        public async Task<bool> ValidateHROwnershipAsync(ClaimsPrincipal user, int hrId)
        {
            // Skip ownership validation if user is admin
            if (IsAdmin(user))
                return true;

            var currentUser = await GetCurrentUserAsync(user);
            return currentUser?.HRId == hrId;
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userIdClaim) ? null : await _userManager.FindByIdAsync(userIdClaim);
        }

        public bool IsAdmin(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value == "Admin";
        }
    }
}
