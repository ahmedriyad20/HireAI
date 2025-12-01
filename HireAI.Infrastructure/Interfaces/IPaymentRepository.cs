using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IPaymentRepository : IGenericRepositoryAsync<Payment>
    {
        // Payment-specific queries
        Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId);
        Task<Payment?> GetByStripeSubscriptionIdAsync(string subscriptionId);
        Task<IEnumerable<Payment>> GetPaymentsByHRIdAsync(int hrId, int take = 50);
        Task<IEnumerable<Payment>> GetSubscriptionPaymentsByHRIdAsync(int hrId);
        Task<Payment?> GetLatestSuccessfulPaymentAsync(int hrId);

        // Subscription management
        Task<HR?> GetHRWithSubscriptionAsync(int hrId);
        Task<BillingInfo?> GetBillingInfoByHRIdAsync(int hrId);
        Task<bool> UpdateHRSubscriptionAsync(int hrId, string subscriptionId, enAccountType accountType, DateTime startDate, DateTime endDate);
        Task<bool> CancelHRSubscriptionAsync(int hrId);

        // Usage tracking
        Task<int> GetJobPostingsUsedAsync(int hrId);
        Task<int> GetApplicantsUsedThisMonthAsync(int hrId);
        Task UpdateHRSubscriptionAsync(int hrId, string id, enAccountType upgradeTo, object currentPeriodStart, object currentPeriodEnd);
    }
}
