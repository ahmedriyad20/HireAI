using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Payment
    {

        public Guid Id { get; set; }
        public string PaymentIntentId { get; set; } // Stripe/PayPal ID
        public string? CustomerId { get; set; } // Stripe Customer ID
        public string? SubscriptionId { get; set; } // Stripe Subscription ID
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public enPaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Subscription details
        public enAccountType UpgradeTo { get; set; }
        public enBillingPeriod BillingPeriod { get; set; }

        // For per-job payments
        public bool IsPerJobPayment { get; set; } = false;
        public int? JobCreditsPurchased { get; set; }
        public decimal? PerJobPrice { get; set; }


        // Invoice information
        public string? InvoiceId { get; set; }
        public string? InvoiceUrl { get; set; }
        public string? ReceiptUrl { get; set; }


        //Foreign Keys
        public int HrId { get; set; }
        public int? PlanId { get; set; }


        //Navigation Property
        public HR? HR { get; set; }
        public virtual SubscriptionPlan? Plan { get; set; }

        // Helper Properties
        [NotMapped]
        public bool IsSubscription => !IsPerJobPayment;

        [NotMapped]
        public bool IsSuccessful => Status == enPaymentStatus.Paid;

        public string StripeSubscriptionId { get; set; }
    }
}
