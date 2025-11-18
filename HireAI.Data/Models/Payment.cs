using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Payment
    {

        public Guid Id { get; set; }
        public string PaymentIntentId { get; set; } // Stripe/PayPal ID
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // What they're upgrading to
        public AccountType UpgradeTo { get; set; }
        public BillingPeriod BillingPeriod { get; set; }

        public int UserId { get; set; } // Foreign key
        public virtual User User { get; set; }

    }
}
