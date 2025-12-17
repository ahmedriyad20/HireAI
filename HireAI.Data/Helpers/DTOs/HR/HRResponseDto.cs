using HireAI.Data.DTOs;
using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.HRDTOS
{
    public class HRResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; }
        public enRole Role { get; set; }
        public bool IsPremium { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }

        // HR-specific
        public string CompanyName { get; set; } = default!;
        public string? CompanyDescription { get; set; }
        public string? CompanyAddress { get; set; } = default!;
        public enAccountType AccountType { get; set; }
        public DateTime? PremiumExpiry { get; set; }

        // If you want to include minimal job info
    }
}
