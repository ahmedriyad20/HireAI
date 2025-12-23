using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Configurations
{
    public static class StripeProducts
    {
        // Product IDs from Stripe Dashboard
        public static readonly Dictionary<enSubscriptionPlan, string> ProductIds = new()
        {
            { enSubscriptionPlan.Starter, "prod_TapKAi79CuHQH0" },
            { enSubscriptionPlan.Professional, "prod_TawnErWStdDwne" }
        };

        // Price IDs - UPDATE these with your actual Stripe price IDs
        public static readonly Dictionary<enSubscriptionPlan, string> PriceIds = new()
        {
            { enSubscriptionPlan.Starter, "price_1Sdku8GI6RzKXyl7DcRcZV33" },
            { enSubscriptionPlan.Professional, "price_1SddfSGI6RzKXyl7O4LmBNQ4" }
        };

        public static string GetProductId(enSubscriptionPlan plan) =>
            ProductIds.TryGetValue(plan, out var id) ? id : throw new ArgumentException($"Invalid plan: {plan}");

        public static string GetPriceId(enSubscriptionPlan plan) =>
            PriceIds.TryGetValue(plan, out var id) ? id : throw new ArgumentException($"Invalid plan: {plan}");
    }
}


