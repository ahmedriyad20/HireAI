using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? ResetCode { get; set; }
        public int? ApplicantId { get; set; }
        public int? HRId { get; set; }
        
        public virtual Applicant? Applicant { get; set; } 
        public virtual HR? HR { get; set; }
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
    }
}
