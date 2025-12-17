namespace HireAI.Data.Helpers.DTOs.Stripe;

public record PaymentMethodResponseDto(
    string Id,
    string Type,
    string? Last4,
    string? Brand,
    long? ExpMonth,
    long? ExpYear,
    bool IsDefault
);
