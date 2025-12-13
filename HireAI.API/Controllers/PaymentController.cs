using HireAI.Data.Helpers.DTOs.Stripe;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HireAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HR")]
public class PaymentController : ControllerBase
{
    private readonly IStripeService _stripeService;
    private readonly HireAIDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PaymentController(
        IStripeService stripeService, 
        HireAIDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _stripeService = stripeService;
        _db = db;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets the current HR user's ID from claims
    /// </summary>
    private async Task<int?> GetCurrentHRIdAsync()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return null;

        var user = await _userManager.FindByIdAsync(userIdClaim);
        return user?.HRId;
    }

    /// <summary>
    /// Creates or retrieves a Stripe customer for the current HR user
    /// </summary>
    [HttpPost("customer/setup")]
    public async Task<ActionResult<CustomerResponseDto>> SetupStripeCustomer()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr == null)
            return NotFound("HR not found");

        // If HR already has a Stripe customer, return it
        if (!string.IsNullOrEmpty(hr.StripeCustomerId))
        {
            var existing = await _stripeService.GetCustomerAsync(hr.StripeCustomerId);
            if (existing != null)
                return Ok(existing);
        }

        // Create new Stripe customer
        var customer = await _stripeService.CreateCustomerAsync(hr.Email, hr.CompanyName);

        // Save StripeCustomerId to HR record
        hr.StripeCustomerId = customer.CustomerId;
        await _db.SaveChangesAsync();

        return Ok(customer);
    }

    /// <summary>
    /// Gets the current HR's Stripe customer info
    /// </summary>
    [HttpGet("customer")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomer()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return NotFound("Stripe customer not setup. Call POST /api/payment/customer/setup first");

        var customer = await _stripeService.GetCustomerAsync(hr.StripeCustomerId);
        return customer != null ? Ok(customer) : NotFound("Stripe customer not found");
    }

    /// <summary>
    /// Creates a setup session to add a new payment method
    /// </summary>
    [HttpPost("payment-method/setup")]
    public async Task<ActionResult<CheckoutSessionResponseDto>> CreateSetupSession()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup. Call POST /api/payment/customer/setup first");

        var session = await _stripeService.CreateSetupSessionAsync(hr.StripeCustomerId);
        return Ok(session);
    }

    /// <summary>
    /// Lists all payment methods for the current HR
    /// </summary>
    [HttpGet("payment-methods")]
    public async Task<ActionResult<List<PaymentMethodResponseDto>>> GetPaymentMethods()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup");

        var methods = await _stripeService.GetPaymentMethodsAsync(hr.StripeCustomerId);
        return Ok(methods);
    }

    /// <summary>
    /// Gets the default payment method
    /// </summary>
    [HttpGet("payment-method/default")]
    public async Task<ActionResult<PaymentMethodResponseDto>> GetDefaultPaymentMethod()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup");

        var method = await _stripeService.GetDefaultPaymentMethodAsync(hr.StripeCustomerId);
        return method != null ? Ok(method) : NotFound("No default payment method set");
    }

    /// <summary>
    /// Sets a payment method as default
    /// </summary>
    [HttpPut("payment-method/default")]
    public async Task<IActionResult> SetDefaultPaymentMethod([FromBody] SetDefaultPaymentMethodRequestDto request)
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup");

        await _stripeService.SetDefaultPaymentMethodAsync(hr.StripeCustomerId, request.PaymentMethodId);
        return Ok(new { message = "Default payment method updated" });
    }

    /// <summary>
    /// Creates a checkout session for subscription
    /// </summary>
    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutSessionResponseDto>> CreateCheckoutSession([FromQuery] enSubscriptionPlan plan)
    {
        if (plan == enSubscriptionPlan.None)
            return BadRequest("Invalid plan selected");

        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup. Call POST /api/payment/customer/setup first");

        var session = await _stripeService.CreateCheckoutSessionAsync(hr.StripeCustomerId, plan);
        return Ok(session);
    }

    /// <summary>
    /// Gets the last successful payment
    /// </summary>
    [HttpGet("last-payment")]
    public async Task<ActionResult<LastPaymentResponseDto>> GetLastPayment()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup");

        var payment = await _stripeService.GetLastPaymentAsync(hr.StripeCustomerId);
        return payment != null ? Ok(payment) : NotFound("No payments found");
    }

    /// <summary>
    /// Gets the current HR's subscription plan
    /// </summary>
    [HttpGet("plan")]
    public async Task<ActionResult<UserPlanResponseDto>> GetCurrentPlan()
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr == null)
            return NotFound("HR not found");

        return Ok(new UserPlanResponseDto(hr.Id, hr.Email, hr.SubscriptionPlan, hr.StripeCustomerId));
    }

    /// <summary>
    /// Changes subscription plan
    /// </summary>
    [HttpPut("plan")]
    public async Task<IActionResult> ChangePlan([FromBody] ChangePlanRequestDto request)
    {
        if (request.NewPlan == enSubscriptionPlan.None)
            return BadRequest("Invalid plan");

        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr?.StripeCustomerId == null)
            return BadRequest("Stripe customer not setup");

        try
        {
            await _stripeService.ChangePlanAsync(hr.StripeCustomerId, request.NewPlan);
            
            // Update local database
            hr.SubscriptionPlan = request.NewPlan;
            hr.AccountType = request.NewPlan == enSubscriptionPlan.None 
                ? enAccountType.Free 
                : enAccountType.Premium;
            hr.PremiumExpiry = DateTime.UtcNow.AddMonths(1); // Adjust based on billing period
            
            await _db.SaveChangesAsync();
            
            return Ok(new { message = $"Plan changed to {request.NewPlan}" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Confirms plan after successful checkout (called from webhook or frontend callback)
    /// </summary>
    [HttpPost("plan/confirm")]
    public async Task<IActionResult> ConfirmPlan([FromQuery] enSubscriptionPlan plan)
    {
        var hrId = await GetCurrentHRIdAsync();
        if (hrId == null)
            return Unauthorized("HR user not found");

        var hr = await _db.HRs.FindAsync(hrId);
        if (hr == null)
            return NotFound("HR not found");

        hr.SubscriptionPlan = plan;
        hr.AccountType = plan == enSubscriptionPlan.None 
            ? enAccountType.Free 
            : enAccountType.Premium;
        hr.PremiumExpiry = DateTime.UtcNow.AddMonths(1);
        
        await _db.SaveChangesAsync();
        
        return Ok(new { message = $"Plan confirmed: {plan}" });
    }
}
