using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        private readonly HireAIDbContext _db;

        public PaymentRepository(HireAIDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // 1. Billing summary: returns HR + User basic info, current plan info (from last payment),
        //    payment method (from last payment), usage (placeholder), and recent payment history.
        //    Returns a plain object to avoid creating DTOs now.
        public async Task<object> GetBillingSummaryAsync(int hrId)
        {
            if (hrId <= 0) return new { HRId = hrId };

            // Load HR (HR inherits User in your model) including any navigation if present.
            var hr = await _db.Set<HR>()
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == hrId);

            // Last payment (most recent by CreatedAt if available, fallback to Id)
            var lastPaymentQuery = _db.Set<Payment>().AsNoTracking().Where(p => p.HrId == hrId);
            Payment? lastPayment;
            try
            {
                lastPayment = await lastPaymentQuery
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                // If CreatedAt is not available on Payment, fallback to Id ordering
                lastPayment = await lastPaymentQuery
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();
            }

            // Recent payments (keep last 12)
            List<Payment> recentPayments;
            try
            {
                recentPayments = await _db.Set<Payment>()
                    .AsNoTracking()
                    .Where(p => p.HrId == hrId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(12)
                    .ToListAsync();
            }
            catch
            {
                recentPayments = await _db.Set<Payment>()
                    .AsNoTracking()
                    .Where(p => p.HrId == hrId)
                    .OrderByDescending(p => p.Id)
                    .Take(12)
                    .ToListAsync();
            }

            // Usage: return basic placeholder (0s) to avoid referencing JobPost/Application entities now.
            var usage = new
            {
                Month = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                JobPostingsCount = 0,
                ApplicantsCount = 0
            };

            // compute CurrentPlan as string to avoid null-coalescing between enum and string
            string currentPlan = "Free";
            if (lastPayment != null)
            {
                // if UpgradeTo is a nullable enum use HasValue / != null checks
                try
                {
                    // safe attempts to convert enum-like fields to string
                    if (lastPayment.UpgradeTo != null)
                        currentPlan = lastPayment.UpgradeTo.ToString() ?? "Free";
                    else if (lastPayment.BillingPeriod != null)
                        currentPlan = lastPayment.BillingPeriod.ToString() ?? "Free";
                }
                catch
                {
                    // fallback to "Free" if conversion fails
                    currentPlan = "Free";
                }
            }

            var result = new
            {
                HR = hr ?? new HR { Id = hrId },
                User = (object?)null, // HR inherits User; if you need User properties pull them from hr
                CurrentPlan = currentPlan,
                LastPayment = lastPayment,
                PaymentMethod = (lastPayment != null ? (object?) (typeof(Payment).GetProperty("PaymentMethod")?.GetValue(lastPayment) ?? null) : null),
                CurrentUsage = usage,
                RecentPayments = recentPayments.AsEnumerable()
            };

            return result;
        }

        // 2. Payment history for last N months - return IEnumerable<Payment>
        public async Task<IEnumerable<Payment>> GetPaymentHistoryAsync(int hrId, int months)
        {
            if (hrId <= 0 || months <= 0) return Enumerable.Empty<Payment>();

            var startDate = DateTime.UtcNow.AddMonths(-months);

            var q = _db.Set<Payment>().AsNoTracking().Where(p => p.HrId == hrId);

            // Try to filter by CreatedAt if available, otherwise return recent payments ordered by Id.
            try
            {
                q = q.Where(p => p.CreatedAt >= startDate);
            }
            catch
            {
                // ignore: CreatedAt not present on Payment
            }

            List<Payment> list;
            try
            {
                list = await q.OrderByDescending(p => p.CreatedAt).ToListAsync();
            }
            catch
            {
                list = await q.OrderByDescending(p => p.Id).ToListAsync();
            }

            return list;
        }

        // 3. Current month usage stats - returns a plain object with Month, JobPostingsCount, ApplicantsCount
        public Task<object> GetCurrentUsageAsync(int hrId)
        {
            var nowUtc = DateTime.UtcNow;
            var monthStart = new DateTime(nowUtc.Year, nowUtc.Month, 1);

            // Avoid referencing JobPost/Application entities; return placeholder zeros for now.
            var usage = new
            {
                HRId = hrId,
                Month = monthStart,
                JobPostingsCount = 0,
                ApplicantsCount = 0
            };

            return Task.FromResult((object)usage);
        }
    }
}



// i will add dto and helper models helpers later 