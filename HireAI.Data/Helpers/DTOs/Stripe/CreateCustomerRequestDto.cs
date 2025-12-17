namespace HireAI.Data.Helpers.DTOs.Stripe;

public record CreateCustomerRequestDto(string Email, string? Name = null);
