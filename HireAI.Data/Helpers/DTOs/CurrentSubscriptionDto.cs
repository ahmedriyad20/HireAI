using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class CurrentSubscriptionDto
    {
        public string PlanName { get; set; } = default!;
        public enAccountType AccountType { get; set; }
        public enBillingPeriod BillingPeriod { get; set; }
        public decimal MonthlyPrice { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsActive { get; set; }

        // Billing Information
        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }
        public DateTime? NextBillingDate { get; set; }

        // Usage Information
        public int JobPostingsUsed { get; set; }
        public int JobPostingsLimit { get; set; } // -1 for unlimited
        public int ApplicantsUsed { get; set; }
        public int ApplicantsLimit { get; set; } // -1 for unlimited

        // Helper Properties
        public bool IsUnlimitedJobs => JobPostingsLimit == -1;
        public bool IsUnlimitedApplicants => ApplicantsLimit == -1;
        public string JobPostingsDisplay => IsUnlimitedJobs ? "Unlimited" : $"{JobPostingsUsed}/{JobPostingsLimit}";
        public string ApplicantsDisplay => IsUnlimitedApplicants ? "Unlimited" : $"{ApplicantsUsed}/{ApplicantsLimit}";

        // Available Job Credits (for per-job purchases)
        public int AvailableJobCredits { get; set; }
    }
}
