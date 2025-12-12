using AutoMapper;
using HireAI.Data.Helpers.DTOs.Respones;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using HireAI.Data.Helpers.Enums;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using HireAI.Infrastructure.Intrefaces;

namespace HireAI.Service.Services
{
    public class HRDashboardService : IHrDashboardService
    {
        private readonly IApplicationRepository _applications;
        private readonly IJobPostRepository _JobPostRepository;
        private readonly IHRRepository _hr;
        private readonly IMapper _map;

        public HRDashboardService(IJobPostRepository jobOpeningRepository,
                         IApplicationRepository applications,
                         IHRRepository hr, IMapper mapper)
        {
            _applications = applications;
           _JobPostRepository = jobOpeningRepository;
            _hr = hr;
            _map = mapper;
        }

        public async Task<HRDashboardDto> GetDashboardAsync(int hrId)
        {
            return new HRDashboardDto
            {
                TotalApplicants = await GetTotalApplicantsAsync(hrId),
                TotalExamTaken = await GetTotalExamTakenAsync(hrId),
                TotalTopApplicants = await GetTotalTopCandidatesAsync(hrId),
                ATSPassedRate = (int)await GetATSPassedRateAsync(hrId),
                MonthlyApplicants = await GetMonthlyNumberOfApplicationsAsync(hrId),
                ATSPassedRateMonthly = await GetMonthlyOfTotalATSPassedAsync(hrId),
                ExamScoreDistribution = await GetExamScoreDistributionAsync(hrId),
                RecentApplications = await GetRecentApplicantsAsync(hrId),
                ActiveJobPostings = await GetActiveJobPostingsAsync(hrId)
            };
        }

        public async Task<int> GetTotalApplicantsAsync(int hrId)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId)
                .CountAsync();
        }

        public async Task<int> GetTotalExamTakenAsync(int hrId)
        {

            return await _applications.GetAll()
                .Where(a => a.HRId == hrId && a.ExamStatus == enExamStatus.Completed)
                .CountAsync();
        }

        public async Task<int> GetTotalTopCandidatesAsync(int hrId)
        {
            return await _applications.GetAll()
                .Include(a => a.ExamSummary)
                .Where(a => a.HRId == hrId &&
                            a.ExamSummary != null &&
                            a.ExamSummary.ApplicantExamScore >= 80 &&
                            a.AtsScore >= 80)
                .CountAsync();
        }

        public async Task<float> GetATSPassedRateAsync(int hrId)
        {
            var totalApplicants = await _applications.GetAll()
                .Where(a => a.HRId == hrId)
                .CountAsync();

            if (totalApplicants == 0)
                return 0;

            var atsPassedCount = await _applications.GetAll()
                .Where(a => a.HRId == hrId && a.ApplicationStatus == enApplicationStatus.ATSPassed)
                .CountAsync();

            return (float)atsPassedCount / totalApplicants * 100;
        }

        public async Task<Dictionary<int, int>> GetMonthlyNumberOfApplicationsAsync(int hrId)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId &&
                            a.DateApplied > DateTime.Now.AddYears(-1))
                .GroupBy(a => a.DateApplied.Month)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }

        public async Task<Dictionary<int, int>> GetMonthlyOfTotalATSPassedAsync(int hrId)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId &&
                            a.ApplicationStatus == enApplicationStatus.UnderReview)
                .GroupBy(a => a.DateApplied.Month)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }

        public async Task<Dictionary<string, float>> GetExamScoreDistributionAsync(int hrId)
        {
            var scoreDistribution = new Dictionary<string, float>
            {
                { "0-20", 0 },
                { "20-40", 0 },
                { "40-60", 0 },
                { "60-80", 0 },
                { "80-100", 0 }
            };

            var totalExamsTaken = await _applications.GetAll()
                .Include(a => a.ExamSummary)
                .ThenInclude(es => es.ExamEvaluation)
                .Where(a => a.HRId == hrId &&
                            a.ExamSummary != null &&
                            a.ExamSummary.ExamEvaluation != null)
                .CountAsync();

            if (totalExamsTaken == 0)
                return scoreDistribution;

            var examScores = await _applications.GetAll()
                .Include(a => a.ExamSummary)
                .ThenInclude(es => es.ExamEvaluation)
                .Where(a => a.HRId == hrId &&
                            a.ExamSummary != null &&
                            a.ExamSummary.ExamEvaluation != null)
                .Select(a => a.ExamSummary.ExamEvaluation.ApplicantExamScore)
                .ToListAsync();

            scoreDistribution["0-20"] = (float)(examScores.Count(s => s >= 0 && s < 20) * 100.0 / totalExamsTaken);
            scoreDistribution["20-40"] = (float)(examScores.Count(s => s >= 20 && s < 40) * 100.0 / totalExamsTaken);
            scoreDistribution["40-60"] = (float)(examScores.Count(s => s >= 40 && s < 60) * 100.0 / totalExamsTaken);
            scoreDistribution["60-80"] = (float)(examScores.Count(s => s >= 60 && s < 80) * 100.0 / totalExamsTaken);
            scoreDistribution["80-100"] = (float)(examScores.Count(s => s >= 80 && s <= 100) * 100.0 / totalExamsTaken);

            return scoreDistribution;
        }

        public async Task<List<RecentApplicationDto>> GetRecentApplicantsAsync(int hrId, int take = 5)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId)
                .Select(a => new RecentApplicationDto
                {
                    ApplicantName = a.Applicant.FullName,
                    ApplicantCVlink = a.CVFilePath,
                    AppliedOn = a.DateApplied,
                    ATSScore = a.AtsScore ?? 0,
                    Position = a.AppliedJob.Title,
                    JobStatus = a.AppliedJob.JobStatus,
                    ExamResultLink = a.ExamSummary != null
                                       ? $"/examsummary/{a.ExamSummary.Id}"
                                       : ""
                })
                .OrderByDescending(a => a.AppliedOn)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<ActiveJobPosting>> GetActiveJobPostingsAsync(int hrId)
        {
            return await _JobPostRepository.GetAll()
                .Where(j => j.HRId == hrId && j.JobStatus == enJobStatus.Active)
                .Select(j => new ActiveJobPosting
                {
                    JobTitle = j.Title,
                    ApplicationTotalCount = j.Applications.Count,
                    JobStatus = j.JobStatus,
                    TakenExamCount = j.Applications.Count(a => a.ExamStatus == enExamStatus.Completed),
                    JobPostLink = $"/jobopenings/{j.Id}"
                })
                .ToListAsync();
        }

    }
}
