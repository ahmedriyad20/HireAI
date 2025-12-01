using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class SubscriptionPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
        public decimal YearlySavings { get; set; }
        public decimal YearlySavingsPercentage { get; set; }
        public enAccountType AccountType { get; set; }

        // Features
        public string JobPostings { get; set; } = default!; // "10", "Unlimited"
        public string Applicants { get; set; } = default!; // "100/month", "Unlimited"
        public List<string> Features { get; set; } = new();

        // Stripe IDs
        public string StripeMonthlyPriceId { get; set; } = default!;
        public string StripeYearlyPriceId { get; set; } = default!;

        // Helper Properties
        public bool IsPopular { get; set; }
        public bool IsCurrentPlan { get; set; }
    }
}
