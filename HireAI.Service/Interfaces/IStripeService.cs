using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireAI.Data.Helpers.DTOs.Stripe;

namespace HireAI.Service.Interfaces;

public interface IStripeService
{
    // Customer management
    Task<CustomerResponseDto> CreateCustomerAsync(string email, string? name = null);
    Task<CustomerResponseDto?> GetCustomerAsync(string customerId);

    // Payment method management
    Task<List<PaymentMethodResponseDto>> GetPaymentMethodsAsync(string customerId);
    Task<PaymentMethodResponseDto?> GetDefaultPaymentMethodAsync(string customerId);
    Task SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId);

    // Checkout sessions
    Task<CheckoutSessionResponseDto> CreateCheckoutSessionAsync(string customerId, enSubscriptionPlan plan);
    Task<CheckoutSessionResponseDto> CreateSetupSessionAsync(string customerId);

    // Payments
    Task<LastPaymentResponseDto?> GetLastPaymentAsync(string customerId);

    // Subscriptions
    Task<string?> GetActiveSubscriptionIdAsync(string customerId);
    Task ChangePlanAsync(string customerId, enSubscriptionPlan newPlan);
}
