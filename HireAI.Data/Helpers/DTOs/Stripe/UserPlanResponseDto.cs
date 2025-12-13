using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Stripe;

public record UserPlanResponseDto(
    int UserId,
    string Email,
    enSubscriptionPlan Plan,
    string? StripeCustomerId
);
