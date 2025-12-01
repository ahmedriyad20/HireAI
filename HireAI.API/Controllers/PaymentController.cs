using HireAI.Data.Helpers.DTOs;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Get current subscription details for the authenticated HR user
        /// </summary>
        [HttpGet("subscription/current")]
        public async Task<ActionResult<CurrentSubscriptionDto>> GetCurrentSubscription()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var subscription = await _paymentService.GetCurrentSubscriptionAsync(hrId);
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current subscription");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get available subscription plans
        /// </summary>
        [HttpGet("plans/available")]
        public async Task<ActionResult<AvailablePlansDto>> GetAvailablePlans()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var plans = await _paymentService.GetAvailablePlansAsync(hrId);
                return Ok(plans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available plans");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get default payment method
        /// </summary>
        [HttpGet("payment-method/default")]
        public async Task<ActionResult<PaymentMethodDto>> GetDefaultPaymentMethod()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var paymentMethod = await _paymentService.GetDefaultPaymentMethodAsync(hrId);

                if (paymentMethod == null)
                    return NotFound(new { message = "No payment method found" });

                return Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default payment method");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Update default payment method
        /// </summary>
        [HttpPut("payment-method/default")]
        public async Task<ActionResult> UpdateDefaultPaymentMethod([FromBody] string paymentMethodId)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var result = await _paymentService.UpdateDefaultPaymentMethodAsync(hrId, paymentMethodId);

                if (!result)
                    return BadRequest(new { error = "Failed to update payment method" });

                return Ok(new { message = "Payment method updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment method");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get billing information
        /// </summary>
        [HttpGet("billing-info")]
        public async Task<ActionResult<BillingInfoDto>> GetBillingInfo()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var billingInfo = await _paymentService.GetBillingInfoAsync(hrId);

                if (billingInfo == null)
                    return NotFound(new { message = "No billing information found" });

                return Ok(billingInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting billing info");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Create or update billing information
        /// </summary>
        [HttpPost("billing-info")]
        public async Task<ActionResult<BillingInfoDto>> CreateOrUpdateBillingInfo([FromBody] CreateBillingInfoDto dto)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var billingInfo = await _paymentService.CreateOrUpdateBillingInfoAsync(hrId, dto);
                return Ok(billingInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating/updating billing info");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Create a payment intent for subscription or per-job payment
        /// </summary>
        [HttpPost("payment-intent")]
        public async Task<ActionResult<PaymentIntentDto>> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var paymentIntent = await _paymentService.CreatePaymentIntentAsync(hrId, dto);
                return Ok(paymentIntent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Create a per-job payment intent
        /// </summary>
        [HttpPost("payment-intent/per-job")]
        public async Task<ActionResult<PaymentIntentDto>> CreatePerJobPaymentIntent([FromBody] int jobCreditsQuantity = 1)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var paymentIntent = await _paymentService.CreatePerJobPaymentIntentAsync(hrId, jobCreditsQuantity);
                return Ok(paymentIntent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating per-job payment intent");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Confirm a payment after successful Stripe payment
        /// </summary>
        [HttpPost("confirm")]
        public async Task<ActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var result = await _paymentService.ConfirmPaymentAsync(hrId, dto);

                if (!result)
                    return BadRequest(new { error = "Payment confirmation failed" });

                return Ok(new { message = "Payment confirmed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Use a job credit (called when posting a new job)
        /// </summary>
        [HttpPost("use-job-credit")]
        public async Task<ActionResult> UseJobCredit()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var result = await _paymentService.UseJobCreditAsync(hrId);

                if (!result)
                    return BadRequest(new { error = "No job credits available" });

                return Ok(new { message = "Job credit used successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using job credit");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get payment history
        /// </summary>
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentHistory([FromQuery] int take = 50)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var history = await _paymentService.GetPaymentHistoryAsync(hrId, take);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment history");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Cancel current subscription
        /// </summary>
        [HttpPost("subscription/cancel")]
        public async Task<ActionResult> CancelSubscription()
        {
            try
            {
                var hrId = GetCurrentHRId();
                var result = await _paymentService.CancelSubscriptionAsync(hrId);

                if (!result)
                    return BadRequest(new { error = "Failed to cancel subscription" });

                return Ok(new { message = "Subscription cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling subscription");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Change subscription plan
        /// </summary>
        [HttpPost("subscription/change")]
        public async Task<ActionResult> ChangeSubscription([FromBody] ChangeSubscriptionRequest request)
        {
            try
            {
                var hrId = GetCurrentHRId();
                var result = await _paymentService.ChangeSubscriptionAsync(hrId, request.PlanId, request.BillingPeriod);

                if (!result)
                    return BadRequest(new { error = "Failed to change subscription" });

                return Ok(new { message = "Subscription changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing subscription");
                return BadRequest(new { error = ex.Message });
            }
        }

        private int GetCurrentHRId()
        {
            // Extract HR ID from JWT token or user claims
            // This is a placeholder - implement based on your authentication system
            var hrIdClaim = User.FindFirst("hr_id") ?? User.FindFirst("sub");
            if (hrIdClaim == null || !int.TryParse(hrIdClaim.Value, out int hrId))
                throw new UnauthorizedAccessException("Invalid HR ID in token");

            return hrId;
        }
    }

    public class ChangeSubscriptionRequest
    {
        public int PlanId { get; set; }
        public enBillingPeriod BillingPeriod { get; set; }
    }
}
