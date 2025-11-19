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
        public HR? HR { get; set; }
        public Applicant? Applicant { get; set; }
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
    }
}
