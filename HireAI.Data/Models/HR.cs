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
        public string CompanyName { get; set; } = null!;
        public AccountType AccountType { get; set; } = AccountType.Free;
        public DateTime? PremiumExpiry { get; set; }


        // Navigation Property
        public virtual ICollection<JobOpening>? HRJobs { get; set; } = new HashSet<JobOpening>(); // jobs created by HR users
        public virtual ICollection<Application>? Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();


    }
}

