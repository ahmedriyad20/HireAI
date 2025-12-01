using HireAI.Data.Helpers.DTOs;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IPaymentService
    {
        // Subscription Management
        Task<CurrentSubscriptionDto> GetCurrentSubscriptionAsync(int hrId);
        Task<AvailablePlansDto> GetAvailablePlansAsync(int hrId);

        // Payment Methods
        Task<PaymentMethodDto> GetDefaultPaymentMethodAsync(int hrId);
        Task<bool> UpdateDefaultPaymentMethodAsync(int hrId, string paymentMethodId);

        // Billing Information
        Task<BillingInfoDto?> GetBillingInfoAsync(int hrId);
        Task<BillingInfoDto> CreateOrUpdateBillingInfoAsync(int hrId, CreateBillingInfoDto dto);

        // Payment Processing
        Task<PaymentIntentDto> CreatePaymentIntentAsync(int hrId, CreatePaymentIntentDto dto);
        Task<bool> ConfirmPaymentAsync(int hrId, ConfirmPaymentDto dto);

        // Per-Job Payments
        Task<PaymentIntentDto> CreatePerJobPaymentIntentAsync(int hrId, int jobCreditsQuantity = 1);
        Task<bool> UseJobCreditAsync(int hrId);

        // Invoice History
        Task<IEnumerable<Payment>> GetPaymentHistoryAsync(int hrId, int take = 50);

        // Subscription Management
        Task<bool> CancelSubscriptionAsync(int hrId);
        Task<bool> ChangeSubscriptionAsync(int hrId, int planId, enBillingPeriod billingPeriod);
    }
}
