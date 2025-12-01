using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HireAI.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        private readonly HireAIDbContext _dbContext;

        public PaymentRepository(HireAIDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public async Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _dbContext.Set<Payment>()
               .Include(p => p.HR)
                .Include(p => p.Plan)
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task<Payment?> GetByStripeSubscriptionIdAsync(string subscriptionId)
        {
            return await _dbContext.Set<Payment>()
                .Include(p => p.HR)
                .Include(p => p.Plan)
                .FirstOrDefaultAsync(p => p.SubscriptionId == subscriptionId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByHRIdAsync(int hrId, int take = 50)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.HrId == hrId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetSubscriptionPaymentsByHRIdAsync(int hrId)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.HrId == hrId && !p.IsPerJobPayment)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payment?> GetLatestSuccessfulPaymentAsync(int hrId)
        {
            return await _dbContext.Set<Payment>()
                .Where(p => p.HrId == hrId && p.Status == Data.Helpers.Enums.enPaymentStatus.Paid)
                .OrderByDescending(p => p.CompletedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<HR?> GetHRWithSubscriptionAsync(int hrId)
        {
            return await _dbContext.HRs
                .Include(h => h.BillingInfo)
                .Include(h => h.Payments)
                .FirstOrDefaultAsync(h => h.Id == hrId);
        }

        public async Task<BillingInfo?> GetBillingInfoByHRIdAsync(int hrId)
        {
            return await _dbContext.BillingInfos
                .FirstOrDefaultAsync(b => b.HRId == hrId);
        }

        public async Task<bool> UpdateHRSubscriptionAsync(int hrId, string subscriptionId, Data.Helpers.Enums.enAccountType accountType, DateTime startDate, DateTime endDate)
        {
            var hr = await _dbContext.HRs.FindAsync(hrId);
            if (hr == null) return false;

            hr.StripeSubscriptionId = subscriptionId;
            hr.AccountType = accountType;
            hr.SubscriptionStartDate = startDate;
            hr.SubscriptionEndDate = endDate;
            hr.PremiumExpiry = endDate;
            hr.IsPremium = accountType != Data.Helpers.Enums.enAccountType.Free;

            _dbContext.HRs.Update(hr);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelHRSubscriptionAsync(int hrId)
        {
            var hr = await _dbContext.HRs.FindAsync(hrId);
            if (hr == null) return false;

            hr.AccountType = Data.Helpers.Enums.enAccountType.Free;
            hr.StripeSubscriptionId = null;
            hr.SubscriptionStartDate = null;
            hr.SubscriptionEndDate = null;
            hr.PremiumExpiry = null;
            hr.IsPremium = false;

            _dbContext.HRs.Update(hr);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> GetJobPostingsUsedAsync(int hrId)
        {
            return await _dbContext.JobOpenings
                .CountAsync(j => j.HRId == hrId && j.JobStatus == Data.Helpers.Enums.enJobStatus.Active);
        }

        public async Task<int> GetApplicantsUsedThisMonthAsync(int hrId)
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _dbContext.Applications
                .CountAsync(a => a.HRId == hrId && a.DateApplied >= startOfMonth);
        }

        Task IPaymentRepository.UpdateHRSubscriptionAsync(int hrId, string id, enAccountType upgradeTo, object currentPeriodStart, object currentPeriodEnd)
        {
            throw new NotImplementedException();
        }
    }
}
