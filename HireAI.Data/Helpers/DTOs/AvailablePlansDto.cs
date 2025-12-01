using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class AvailablePlansDto
    {
        public List<SubscriptionPlanDto> MonthlyPlans { get; set; } = new();
        public List<SubscriptionPlanDto> YearlyPlans { get; set; } = new();
        public PerJobOptionDto PerJobOption { get; set; } = new();
        public string? CurrentPlanId { get; set; }
    }

    public class PerJobOptionDto
    {
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public string StripePriceId { get; set; } = default!;
        public bool IsAvailable { get; set; } = true;
    }
}
