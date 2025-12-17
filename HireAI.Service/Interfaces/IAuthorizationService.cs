using HireAI.Data.Models.Identity;
using System.Security.Claims;


namespace HireAI.Service.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> ValidateApplicantOwnershipAsync(ClaimsPrincipal user, int applicantId);
        Task<bool> ValidateHROwnershipAsync(ClaimsPrincipal user, int hrId);
        Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal user);
        public bool IsAdmin(ClaimsPrincipal user);
    }
}
