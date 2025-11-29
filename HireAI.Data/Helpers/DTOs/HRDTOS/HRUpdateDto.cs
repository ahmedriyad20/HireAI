using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.HRDTOS
{
    public class HRUpdateDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }

        public string? CompanyName { get; set; }
        public enAccountType? AccountType { get; set; }
    }
}
