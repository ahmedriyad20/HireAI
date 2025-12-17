namespace HireAI.Data.Helpers.DTOs.Stripe;

public record LastPaymentResponseDto(
    string Id,
    long Amount,
    string Currency,
    string Status,
    DateTime Created,
    string? PaymentMethodLast4
);
