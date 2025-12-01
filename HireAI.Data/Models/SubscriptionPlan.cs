using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    /// <summary>
    /// Represents a subscription plan available for HR users
    /// </summary>
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!; // Starter, Professional, Enterprise
        public string Description { get; set; } = default!;
       
        // Pricing
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
        public decimal? PerJobPrice { get; set; } = 49.00m; // Default per-job price



        // Plan Features
        public enAccountType AccountType { get; set; }
        public int JobPostingsLimit { get; set; } // -1 for unlimited
        public int ApplicantsLimit { get; set; } // -1 for unlimited
        public bool HasAIScreening { get; set; }
        public bool HasAdvancedAIFeatures { get; set; }
        public bool HasPrioritySupport { get; set; }
        public bool HasCustomBranding { get; set; }
        public bool HasDedicatedSupport { get; set; } // For Enterprise
        public bool HasCustomIntegrations { get; set; } // For Enterprise
        public bool IsActive { get; set; } = true;



        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;



        // Stripe Price IDs for monthly and yearly plans
        public string? StripeMonthlyPriceId { get; set; }
        public string? StripeYearlyPriceId { get; set; }
        public string? StripePerJobPriceId { get; set; }

        // Management
        public int DisplayOrder { get; set; } = 0;


        // Helper Properties
        public decimal YearlySavings => MonthlyPrice * 12 - YearlyPrice;
        public decimal YearlySavingsPercentage => MonthlyPrice > 0 ? (YearlySavings / (MonthlyPrice * 12)) * 100 : 0;

        public bool IsUnlimitedJobs => JobPostingsLimit == -1;
        public bool IsUnlimitedApplicants => ApplicantsLimit == -1;

        // Navigation Property
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
