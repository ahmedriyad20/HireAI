using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models 
{ 
    public class HR : User
    {
        public string CompanyName { get; set; } = default!;
        public enAccountType AccountType { get; set; } = enAccountType.Free;
        public DateTime? PremiumExpiry { get; set; }



        // Subscription Management
        public string? StripeCustomerId { get; set; }
        public string? StripeSubscriptionId { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        // Job Credits for Per-Job payments
        public int AvailableJobCredits { get; set; } = 0;


        // Navigation Property
        public virtual ICollection<JobOpening> HRJobs { get; set; } = new HashSet<JobOpening>(); // jobs created by HR users
        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public virtual BillingInfo? BillingInfo { get; set; }


        // Helper Properties
        [NotMapped]
        public bool HasActiveSubscription =>
            AccountType != enAccountType.Free &&
            SubscriptionEndDate.HasValue &&
            SubscriptionEndDate.Value > DateTime.UtcNow;

        [NotMapped]
        public bool CanPostJobs =>
            HasActiveSubscription || AvailableJobCredits > 0;
    }
}

