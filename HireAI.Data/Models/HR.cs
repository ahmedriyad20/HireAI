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
        public string? CompanyDescription { get; set; }
        public string? CompanyAddress { get; set; } = default!;
        public enAccountType AccountType { get; set; } = enAccountType.Free;
        public DateTime? PremiumExpiry { get; set; }
        public string? StripeCustomerId { get; set; } 
        public enSubscriptionPlan SubscriptionPlan { get; set; } = enSubscriptionPlan.None; 


        // Navigation Property
        public virtual ICollection<JobPost> HRJobs { get; set; } = new HashSet<JobPost>(); // jobs created by HR users
        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
    }
}

