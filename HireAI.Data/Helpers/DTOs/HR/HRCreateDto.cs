using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.HRDTOS
{
    public class HRCreateDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }

        [Required]
        public string CompanyName { get; set; } = default!;

        public enAccountType AccountType { get; set; } = enAccountType.Free;
    }

}
