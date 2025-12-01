using HireAI.Data.Helpers.DTOs;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireAI.Service.Implementions
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IGenericRepositoryAsync<SubscriptionPlan> _planRepository;
        private readonly IGenericRepositoryAsync<BillingInfo> _billingInfoRepository;
        private readonly IGenericRepositoryAsync<HR> _hrRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;

        // Stripe Configuration
        private readonly string _stripeSecretKey;
        private readonly string _stripePublishableKey;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IGenericRepositoryAsync<SubscriptionPlan> planRepository,
            IGenericRepositoryAsync<BillingInfo> billingInfoRepository,
            IGenericRepositoryAsync<HR> hrRepository,
            IConfiguration configuration,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _planRepository = planRepository;
            _billingInfoRepository = billingInfoRepository;
            _hrRepository = hrRepository;
            _configuration = configuration;
            _logger = logger;

            // Configure Stripe
            _stripeSecretKey = _configuration["Stripe:SecretKey"] ?? throw new ArgumentNullException("Stripe:SecretKey");
            _stripePublishableKey = _configuration["Stripe:PublishableKey"] ?? throw new ArgumentNullException("Stripe:PublishableKey");

            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task<CurrentSubscriptionDto> GetCurrentSubscriptionAsync(int hrId)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                if (hr == null)
                    throw new ArgumentException($"HR with ID {hrId} not found");

                var currentPlan = await GetCurrentPlanAsync(hr);
                var jobPostingsUsed = await _paymentRepository.GetJobPostingsUsedAsync(hrId);
                var applicantsUsed = await _paymentRepository.GetApplicantsUsedThisMonthAsync(hrId);

                return new CurrentSubscriptionDto
                {
                    PlanName = currentPlan?.Name ?? "Free Plan",
                    AccountType = hr.AccountType,
                    BillingPeriod = GetBillingPeriodFromSubscription(hr),
                    MonthlyPrice = currentPlan?.MonthlyPrice ?? 0,
                    Status = hr.HasActiveSubscription ? "Active" : "Inactive",
                    IsActive = hr.HasActiveSubscription,
                    CurrentPeriodStart = hr.SubscriptionStartDate,
                    CurrentPeriodEnd = hr.SubscriptionEndDate,
                    NextBillingDate = hr.SubscriptionEndDate,
                    JobPostingsUsed = jobPostingsUsed,
                    JobPostingsLimit = currentPlan?.JobPostingsLimit ?? 1, // Free plan has 1 job posting
                    ApplicantsUsed = applicantsUsed,
                    ApplicantsLimit = currentPlan?.ApplicantsLimit ?? 10, // Free plan has 10 applicants
                    AvailableJobCredits = hr.AvailableJobCredits
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current subscription for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<AvailablePlansDto> GetAvailablePlansAsync(int hrId)
        {
            try
            {
                //var activePlans = (await _planRepository.GetAllAsync())
                //    .Where(p => p.IsActive)
                //    .OrderBy(p => p.DisplayOrder)
                //    .ToList();
                // Use GetAll() instead of GetAllAsync()
                var activePlans = _planRepository.GetAll()  // Remove await, use synchronous GetAll()
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .ToList();  // This is now synchronous

                var currentSubscription = await GetCurrentSubscriptionAsync(hrId);
                var perJobPlan = activePlans.FirstOrDefault(p => p.PerJobPrice.HasValue);

                var monthlyPlans = activePlans
                    .Where(p => p.MonthlyPrice > 0)
                    .Select(p => MapToSubscriptionPlanDto(p, enBillingPeriod.Monthly,
                        currentSubscription.PlanName == p.Name && currentSubscription.BillingPeriod == enBillingPeriod.Monthly))
                    .ToList();

                var yearlyPlans = activePlans
                    .Where(p => p.YearlyPrice > 0)
                    .Select(p => MapToSubscriptionPlanDto(p, enBillingPeriod.Yearly,
                        currentSubscription.PlanName == p.Name && currentSubscription.BillingPeriod == enBillingPeriod.Yearly))
                    .ToList();

                return new AvailablePlansDto
                {
                    MonthlyPlans = monthlyPlans,
                    YearlyPlans = yearlyPlans,
                    PerJobOption = new PerJobOptionDto
                    {
                        Price = perJobPlan?.PerJobPrice ?? 49.00m,
                        Description = "Need to hire just once? Purchase individual job credits without a subscription.",
                        StripePriceId = perJobPlan?.StripePerJobPriceId,
                        IsAvailable = true
                    },
                    CurrentPlanId = currentSubscription.IsActive ? currentSubscription.PlanName : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available plans for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<PaymentMethodDto> GetDefaultPaymentMethodAsync(int hrId)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                if (hr == null || string.IsNullOrEmpty(hr.StripeCustomerId))
                    return null;

                var customerService = new CustomerService();
                var customer = await customerService.GetAsync(hr.StripeCustomerId);

                if (string.IsNullOrEmpty(customer.InvoiceSettings?.DefaultPaymentMethodId))
                    return null;

                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.GetAsync(customer.InvoiceSettings.DefaultPaymentMethodId);

                return MapToPaymentMethodDto(paymentMethod);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error getting payment method for HR {HRId}", hrId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment method for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<bool> UpdateDefaultPaymentMethodAsync(int hrId, string paymentMethodId)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                if (hr == null || string.IsNullOrEmpty(hr.StripeCustomerId))
                    return false;

                var customerService = new CustomerService();
                var options = new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId
                    }
                };

                await customerService.UpdateAsync(hr.StripeCustomerId, options);
                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error updating payment method for HR {HRId}", hrId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment method for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<BillingInfoDto?> GetBillingInfoAsync(int hrId)
        {
            try
            {
                var billingInfo = await _paymentRepository.GetBillingInfoByHRIdAsync(hrId);
                return billingInfo != null ? MapToBillingInfoDto(billingInfo) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting billing info for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<BillingInfoDto> CreateOrUpdateBillingInfoAsync(int hrId, CreateBillingInfoDto dto)
        {
            try
            {
                var existingBillingInfo = await _paymentRepository.GetBillingInfoByHRIdAsync(hrId);

                if (existingBillingInfo != null)
                {
                    // Update existing
                    existingBillingInfo.CompanyName = dto.CompanyName;
                    existingBillingInfo.AddressLine1 = dto.AddressLine1;
                    existingBillingInfo.AddressLine2 = dto.AddressLine2;
                    existingBillingInfo.City = dto.City;
                    existingBillingInfo.State = dto.State;
                    existingBillingInfo.Country = dto.Country;
                    existingBillingInfo.PostalCode = dto.PostalCode;
                    existingBillingInfo.Website = dto.Website;
                    existingBillingInfo.TaxId = dto.TaxId;
                    existingBillingInfo.UpdatedAt = DateTime.UtcNow;

                    await _billingInfoRepository.UpdateAsync(existingBillingInfo);
                    return MapToBillingInfoDto(existingBillingInfo);
                }
                else
                {
                    // Create new
                    var newBillingInfo = new BillingInfo
                    {
                        HRId = hrId,
                        CompanyName = dto.CompanyName,
                        AddressLine1 = dto.AddressLine1,
                        AddressLine2 = dto.AddressLine2,
                        City = dto.City,
                        State = dto.State,
                        Country = dto.Country,
                        PostalCode = dto.PostalCode,
                        Website = dto.Website,
                        TaxId = dto.TaxId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _billingInfoRepository.AddAsync(newBillingInfo);
                    return MapToBillingInfoDto(newBillingInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating/updating billing info for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<PaymentIntentDto> CreatePaymentIntentAsync(int hrId, CreatePaymentIntentDto dto)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                var plan = await _planRepository.GetByIdAsync(dto.PlanId);

                if (plan == null)
                    throw new ArgumentException($"Plan with ID {dto.PlanId} not found");

                // Get or create Stripe customer
                var customerId = await GetOrCreateStripeCustomerAsync(hr);

                // Create payment intent
                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = CalculateAmount(plan, dto.BillingPeriod, dto.IsPerJobPayment, dto.JobCreditsQuantity),
                    Currency = "usd",
                    Customer = customerId,
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "hr_id", hrId.ToString() },
                        { "plan_id", dto.PlanId.ToString() },
                        { "billing_period", dto.BillingPeriod.ToString() },
                        { "is_per_job", dto.IsPerJobPayment.ToString() },
                        { "job_credits", dto.JobCreditsQuantity?.ToString() ?? "1" }
                    }
                };

                var paymentIntent = await paymentIntentService.CreateAsync(options);

                // Save payment record
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    PaymentIntentId = paymentIntent.Id,
                    CustomerId = customerId,
                    HrId = hrId,
                    PlanId = dto.PlanId,
                    Amount = Convert.ToDecimal(paymentIntent.Amount) / 100, // Convert from cents
                    Currency = paymentIntent.Currency,
                    Status = enPaymentStatus.Pending,
                    UpgradeTo = plan.AccountType,
                    BillingPeriod = dto.BillingPeriod,
                    IsPerJobPayment = dto.IsPerJobPayment,
                    JobCreditsPurchased = dto.JobCreditsQuantity,
                    PerJobPrice = dto.IsPerJobPayment ? plan.PerJobPrice : null,
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRepository.AddAsync(payment);

                return new PaymentIntentDto
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id,
                    Amount = payment.Amount,
                    Currency = paymentIntent.Currency,
                    Status = paymentIntent.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<PaymentIntentDto> CreatePerJobPaymentIntentAsync(int hrId, int jobCreditsQuantity = 1)
        {
            var perJobPlan = _planRepository.GetAll()
                .FirstOrDefault(p => p.PerJobPrice.HasValue);

            if (perJobPlan == null)
                throw new InvalidOperationException("Per-job payment plan not configured");

            var dto = new CreatePaymentIntentDto
            {
                PlanId = perJobPlan.Id,
                BillingPeriod = enBillingPeriod.oneTime,
                IsPerJobPayment = true,
                JobCreditsQuantity = jobCreditsQuantity
            };

            return await CreatePaymentIntentAsync(hrId, dto);
        }

        public async Task<bool> ConfirmPaymentAsync(int hrId, ConfirmPaymentDto dto)
        {
            try
            {
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.GetAsync(dto.PaymentIntentId);

                if (paymentIntent.Status != "succeeded")
                    return false;

                // Update payment record
                var payment = await _paymentRepository.GetByPaymentIntentIdAsync(dto.PaymentIntentId);
                if (payment == null || payment.HrId != hrId)
                    return false;

                payment.Status = Data.Helpers.Enums.enPaymentStatus.Paid;
                payment.CompletedAt = DateTime.UtcNow;
                payment.InvoiceId = paymentIntent.LatestChargeId;
                //payment.InvoiceId = paymentIntent.InvoiceId;
                //payment.ReceiptUrl = paymentIntent.Charges.Data.FirstOrDefault()?.ReceiptUrl;
                if (!string.IsNullOrEmpty(paymentIntent.LatestChargeId))
{
    var chargeService = new ChargeService();
    var charge = await chargeService.GetAsync(paymentIntent.LatestChargeId);
    payment.ReceiptUrl = charge.ReceiptUrl;
}
else
{
    payment.ReceiptUrl = null;
}


                await _paymentRepository.UpdateAsync(payment);

                // Update HR account based on payment type
                if (payment.IsPerJobPayment)
                {
                    // Add job credits
                    var hr = await _hrRepository.GetByIdAsync(hrId);
                    hr.AvailableJobCredits += payment.JobCreditsPurchased ?? 1;
                    await _hrRepository.UpdateAsync(hr);
                }
                else
                {



                    // Create or update subscription
                    var subscriptionService = new SubscriptionService();
                    var subscription = await subscriptionService.GetAsync(payment.StripeSubscriptionId);

                    await _paymentRepository.UpdateHRSubscriptionAsync(
                        hrId,
                        subscription.Id,
                        payment.UpgradeTo,
                        subscription.StartDate,
                        subscription.EndedAt
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<bool> UseJobCreditAsync(int hrId)
        {
            try
            {
                var hr = await _hrRepository.GetByIdAsync(hrId);
                if (hr == null || hr.AvailableJobCredits <= 0)
                    return false;

                hr.AvailableJobCredits--;
                await _hrRepository.UpdateAsync(hr);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using job credit for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> GetPaymentHistoryAsync(int hrId, int take = 50)
        {
            try
            {
                return await _paymentRepository.GetPaymentsByHRIdAsync(hrId, take);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment history for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<bool> CancelSubscriptionAsync(int hrId)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                if (hr == null || string.IsNullOrEmpty(hr.StripeSubscriptionId))
                    return false;

                var subscriptionService = new SubscriptionService();
                await subscriptionService.CancelAsync(hr.StripeSubscriptionId);

                await _paymentRepository.CancelHRSubscriptionAsync(hrId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling subscription for HR {HRId}", hrId);
                throw;
            }
        }

        public async Task<bool> ChangeSubscriptionAsync(int hrId, int planId, enBillingPeriod billingPeriod)
        {
            try
            {
                var hr = await _paymentRepository.GetHRWithSubscriptionAsync(hrId);
                var plan = await _planRepository.GetByIdAsync(planId);

                if (hr == null || plan == null || string.IsNullOrEmpty(hr.StripeSubscriptionId))
                    return false;

                var subscriptionService = new SubscriptionService();
                var subscription = await subscriptionService.GetAsync(hr.StripeSubscriptionId);

                var priceId = billingPeriod == enBillingPeriod.Monthly
                    ? plan.StripeMonthlyPriceId
                    : plan.StripeYearlyPriceId;

                var options = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Id = subscription.Items.Data[0].Id,
                            Price = priceId
                        }
                    },
                    ProrationBehavior = "create_prorations"
                };

                await subscriptionService.UpdateAsync(hr.StripeSubscriptionId, options);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing subscription for HR {HRId}", hrId);
                throw;
            }
        }

        #region Private Helper Methods

        private async Task<SubscriptionPlan?> GetCurrentPlanAsync(HR hr)
        {
            if (!hr.HasActiveSubscription)
                return null;

            var plans = _planRepository.GetAll(); // Use GetAll() instead of GetAllAsync()
            //var plans = await _planRepository.GetAllAsync();
            return plans.FirstOrDefault(p => p.AccountType == hr.AccountType);
        }

        private enBillingPeriod GetBillingPeriodFromSubscription(HR hr)
        {
            if (!hr.SubscriptionStartDate.HasValue || !hr.SubscriptionEndDate.HasValue)
                return enBillingPeriod.Monthly;

            var months = (hr.SubscriptionEndDate.Value.Year - hr.SubscriptionStartDate.Value.Year) * 12
                + hr.SubscriptionEndDate.Value.Month - hr.SubscriptionStartDate.Value.Month;

            return months >= 12 ? enBillingPeriod.Yearly : enBillingPeriod.Monthly;
        }

        private SubscriptionPlanDto MapToSubscriptionPlanDto(SubscriptionPlan plan, enBillingPeriod billingPeriod, bool isCurrentPlan)
        {
            var price = billingPeriod == enBillingPeriod.Monthly ? plan.MonthlyPrice : plan.YearlyPrice;
            var stripePriceId = billingPeriod == enBillingPeriod.Monthly ? plan.StripeMonthlyPriceId : plan.StripeYearlyPriceId;

            var features = new List<string>();
            if (plan.HasAIScreening) features.Add("AI screening");
            if (plan.HasAdvancedAIFeatures) features.Add("Advanced AI features");
            if (plan.HasPrioritySupport) features.Add("Priority support");
            if (plan.HasCustomBranding) features.Add("Custom branding");
            if (plan.HasDedicatedSupport) features.Add("Dedicated support");
            if (plan.HasCustomIntegrations) features.Add("Custom integrations");

            return new SubscriptionPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                MonthlyPrice = plan.MonthlyPrice,
                YearlyPrice = plan.YearlyPrice,
                YearlySavings = plan.YearlySavings,
                YearlySavingsPercentage = plan.YearlySavingsPercentage,
                AccountType = plan.AccountType,
                JobPostings = plan.IsUnlimitedJobs ? "Unlimited" : plan.JobPostingsLimit.ToString(),
                Applicants = plan.IsUnlimitedApplicants ? "Unlimited" : $"{plan.ApplicantsLimit}/month",
                Features = features,
                StripeMonthlyPriceId = plan.StripeMonthlyPriceId,
                StripeYearlyPriceId = plan.StripeYearlyPriceId,
                IsPopular = plan.Name == "Professional",
                IsCurrentPlan = isCurrentPlan
            };
        }

        private PaymentMethodDto MapToPaymentMethodDto(PaymentMethod paymentMethod)
        {
            return new PaymentMethodDto
            {
                Id = paymentMethod.Id,
                Type = paymentMethod.Type,
                Card = new CardInfoDto
                {
                    Brand = paymentMethod.Card.Brand,
                    Last4 = paymentMethod.Card.Last4,
                    ExpMonth = paymentMethod.Card.ExpMonth,
                    ExpYear = paymentMethod.Card.ExpYear,
                    Country = paymentMethod.Card.Country
                },
                IsDefault = true // Since we're getting the default payment method
            };
        }

        private BillingInfoDto MapToBillingInfoDto(BillingInfo billingInfo)
        {
            return new BillingInfoDto
            {
                Id = billingInfo.Id,
                CompanyName = billingInfo.CompanyName,
                AddressLine1 = billingInfo.AddressLine1,
                AddressLine2 = billingInfo.AddressLine2,
                City = billingInfo.City,
                State = billingInfo.State,
                Country = billingInfo.Country,
                PostalCode = billingInfo.PostalCode,
                Website = billingInfo.Website,
                TaxId = billingInfo.TaxId
            };
        }

        private async Task<string> GetOrCreateStripeCustomerAsync(HR hr)
        {
            if (!string.IsNullOrEmpty(hr.StripeCustomerId))
                return hr.StripeCustomerId;

            var customerService = new CustomerService();
            var customerOptions = new CustomerCreateOptions
            {
                Email = hr.Email,
                Name = hr.CompanyName,
                Metadata = new Dictionary<string, string>
                {
                    { "hr_id", hr.Id.ToString() }
                }
            };

            var customer = await customerService.CreateAsync(customerOptions);

            // Update HR record with Stripe customer ID
            hr.StripeCustomerId = customer.Id;
            await _hrRepository.UpdateAsync(hr);

            return customer.Id;
        }

        private long CalculateAmount(SubscriptionPlan plan, enBillingPeriod billingPeriod, bool isPerJobPayment, int? jobCreditsQuantity)
        {
            if (isPerJobPayment)
            {
                var total = (plan.PerJobPrice ?? 49.00m) * (jobCreditsQuantity ?? 1);
                return Convert.ToInt64(total * 100); // Convert to cents
            }

            var amount = billingPeriod == enBillingPeriod.Monthly ? plan.MonthlyPrice : plan.YearlyPrice;
            return Convert.ToInt64(amount * 100); // Convert to cents
        }

        #endregion
    }
}