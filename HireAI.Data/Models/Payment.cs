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
        public enPaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        // What they're upgrading to
        public enAccountType UpgradeTo { get; set; }
        public enBillingPeriod BillingPeriod { get; set; }

        //Foreign Keys
        public int HrId { get; set; }

        //Navigation Property
        public HR? HR { get; set; }

    }
}
