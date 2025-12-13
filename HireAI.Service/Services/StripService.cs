using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using HireAI.Service.Interfaces;
using HireAI.Data.Helpers.DTOs.Stripe;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Helpers.Configurations;

namespace HireAI.Service.Services;


    


// Stripe service implementation - Stripe is the SINGLE source of truth
public class StripeService : IStripeService
{
    private readonly StripeSettings _settings;
    private readonly CustomerService _customerService;
    private readonly PaymentMethodService _paymentMethodService;
    private readonly SessionService _sessionService;
    private readonly PaymentIntentService _paymentIntentService;
    private readonly SubscriptionService _subscriptionService;

    public StripeService(IOptions<StripeSettings> settings)
    {
        _settings = settings.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;

        _customerService = new CustomerService();
        _paymentMethodService = new PaymentMethodService();
        _sessionService = new SessionService();
        _paymentIntentService = new PaymentIntentService();
        _subscriptionService = new SubscriptionService();
    }

    // Creates a new Stripe customer
    public async Task<CustomerResponseDto> CreateCustomerAsync(string email, string? name = null)
    {
        var options = new CustomerCreateOptions
        {
            Email = email,
            Name = name
        };

        var customer = await _customerService.CreateAsync(options);
        return new CustomerResponseDto(customer.Id, customer.Email, customer.Name);
    }

    // Retrieves existing customer from Stripe
    public async Task<CustomerResponseDto?> GetCustomerAsync(string customerId)
    {
        try
        {
            var customer = await _customerService.GetAsync(customerId);
            return new CustomerResponseDto(customer.Id, customer.Email, customer.Name);
        }
        catch (StripeException)
        {
            return null;
        }
    }

    // Gets all payment methods for a customer - fetched live from Stripe
    public async Task<List<PaymentMethodResponseDto>> GetPaymentMethodsAsync(string customerId)
    {
        var customer = await _customerService.GetAsync(customerId, new CustomerGetOptions
        {
            Expand = ["invoice_settings.default_payment_method"]
        });

        var defaultPaymentMethodId = customer.InvoiceSettings?.DefaultPaymentMethodId;

        var options = new PaymentMethodListOptions
        {
            Customer = customerId
        };

        var paymentMethods = await _paymentMethodService.ListAsync(options);

        return paymentMethods.Data.Select(pm => new PaymentMethodResponseDto(
            pm.Id,
            pm.Type,
            pm.Card?.Last4,
            pm.Card?.Brand,
            pm.Card?.ExpMonth,
            pm.Card?.ExpYear,
            pm.Id == defaultPaymentMethodId
        )).ToList();
    }

    // Gets the default payment method from Stripe
    public async Task<PaymentMethodResponseDto?> GetDefaultPaymentMethodAsync(string customerId)
    {
        var customer = await _customerService.GetAsync(customerId, new CustomerGetOptions
        {
            Expand = ["invoice_settings.default_payment_method"]
        });

        var defaultPm = customer.InvoiceSettings?.DefaultPaymentMethod;
        if (defaultPm == null) return null;

        return new PaymentMethodResponseDto(
            defaultPm.Id,
            defaultPm.Type,
            defaultPm.Card?.Last4,
            defaultPm.Card?.Brand,
            defaultPm.Card?.ExpMonth,
            defaultPm.Card?.ExpYear,
            true
        );
    }

    // Sets the default payment method in Stripe
    public async Task SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId)
    {
        var options = new CustomerUpdateOptions
        {
            InvoiceSettings = new CustomerInvoiceSettingsOptions
            {
                DefaultPaymentMethod = paymentMethodId
            }
        };

        await _customerService.UpdateAsync(customerId, options);
    }

    // Creates a checkout session for subscription
    public async Task<CheckoutSessionResponseDto> CreateCheckoutSessionAsync(string customerId, enSubscriptionPlan plan)
    {
        var priceId = StripeProducts.GetPriceId(plan);

        var options = new SessionCreateOptions
        {
            Customer = customerId,
            Mode = "subscription",
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1
                }
            ],
            SuccessUrl = _settings.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = _settings.CancelUrl,
            PaymentMethodTypes = ["card"]
        };

        var session = await _sessionService.CreateAsync(options);
        return new CheckoutSessionResponseDto(session.Id, session.Url);
    }

    // Creates a setup session for adding payment methods without charging
    public async Task<CheckoutSessionResponseDto> CreateSetupSessionAsync(string customerId)
    {
        var options = new SessionCreateOptions
        {
            Customer = customerId,
            Mode = "setup",
            SuccessUrl = _settings.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = _settings.CancelUrl,
            PaymentMethodTypes = ["card"]
        };

        var session = await _sessionService.CreateAsync(options);
        return new CheckoutSessionResponseDto(session.Id, session.Url);
    }

    // Gets the last successful payment from Stripe
    public async Task<LastPaymentResponseDto?> GetLastPaymentAsync(string customerId)
    {
        var options = new PaymentIntentListOptions
        {
            Customer = customerId,
            Limit = 10
        };

        var paymentIntents = await _paymentIntentService.ListAsync(options);
        var lastSuccessful = paymentIntents.Data
            .Where(pi => pi.Status == "succeeded")
            .OrderByDescending(pi => pi.Created)
            .FirstOrDefault();

        if (lastSuccessful == null) return null;

        string? last4 = null;
        if (!string.IsNullOrEmpty(lastSuccessful.PaymentMethodId))
        {
            try
            {
                var pm = await _paymentMethodService.GetAsync(lastSuccessful.PaymentMethodId);
                last4 = pm.Card?.Last4;
            }
            catch { /* Payment method may have been removed */ }
        }

        return new LastPaymentResponseDto(
            lastSuccessful.Id,
            lastSuccessful.Amount,
            lastSuccessful.Currency,
            lastSuccessful.Status,
            lastSuccessful.Created,
            last4
        );
    }

    // Gets the active subscription ID for a customer
    public async Task<string?> GetActiveSubscriptionIdAsync(string customerId)
    {
        var options = new SubscriptionListOptions
        {
            Customer = customerId,
            Status = "active",
            Limit = 1
        };

        var subscriptions = await _subscriptionService.ListAsync(options);
        return subscriptions.Data.FirstOrDefault()?.Id;
    }

    // Changes the subscription plan
    public async Task ChangePlanAsync(string customerId, enSubscriptionPlan newPlan)
    {
        var subscriptionId = await GetActiveSubscriptionIdAsync(customerId);
        if (subscriptionId == null)
            throw new InvalidOperationException("No active subscription found");

        var subscription = await _subscriptionService.GetAsync(subscriptionId);
        var itemId = subscription.Items.Data.FirstOrDefault()?.Id
            ?? throw new InvalidOperationException("No subscription item found");

        var newPriceId = StripeProducts.GetPriceId(newPlan);

        var options = new SubscriptionUpdateOptions
        {
            Items =
            [
                new SubscriptionItemOptions
                {
                    Id = itemId,
                    Price = newPriceId
                }
            ],
            ProrationBehavior = "create_prorations"
        };

        await _subscriptionService.UpdateAsync(subscriptionId, options);
    }


}
