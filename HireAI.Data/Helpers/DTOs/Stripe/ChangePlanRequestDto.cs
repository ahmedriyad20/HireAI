using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Stripe;

public record ChangePlanRequestDto(enSubscriptionPlan NewPlan);
