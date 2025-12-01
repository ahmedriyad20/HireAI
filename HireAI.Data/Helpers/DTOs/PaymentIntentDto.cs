using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class PaymentIntentDto
    {
        public string ClientSecret { get; set; } = default!;
        public string PaymentIntentId { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string Status { get; set; } = default!;
    }

    public class CreatePaymentIntentDto
    {
        public int PlanId { get; set; }
        public enBillingPeriod BillingPeriod { get; set; }
        public bool IsPerJobPayment { get; set; } = false;
        public int? JobCreditsQuantity { get; set; } = 1;
    }

    public class ConfirmPaymentDto
    {
        public string PaymentIntentId { get; set; } = default!;
        public string? PaymentMethodId { get; set; }
    }
}
