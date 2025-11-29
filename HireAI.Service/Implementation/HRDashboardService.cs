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

namespace HireAI.Service.Implementation
{
    public class HRDashboardService : IHRDashboardService
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
                TotalTopCandidates = await GetTotalTopCandidatesAsync(hrId),
                MonthlyApplicants = await GetMonthlyNumberOfApplicationsAsync(hrId),
                ATSPassedRateMonthly = await GetMonthlyOfTotalATSPassedAsync(hrId),
                RecentApplications = await GetRecentApplicantsAsync(hrId),
                ActiveJopPostings = await GetActiveJobPostingsAsync(hrId)
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
                .Where(a => a.HRId == hrId && a.ExamStatus == enExamStatus.completed)
                .CountAsync();
        }

        public async Task<int> GetTotalTopCandidatesAsync(int hrId)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId &&
                            a.ExamSummary != null &&
                            a.ExamSummary.TotalScroe >= 80 &&
                            a.AtsScore >= 80)
                .CountAsync();
        }

        public async Task<Dictionary<int, int>> GetMonthlyNumberOfApplicationsAsync(int hrId)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId &&
                            a.DateApplied > DateTime.UtcNow.AddYears(-1))
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

        public async Task<List<RecentApplicationDto>> GetRecentApplicantsAsync(int hrId, int take = 5)
        {
            return await _applications.GetAll()
                .Where(a => a.HRId == hrId)
                .Select(a => new RecentApplicationDto
                {
                    ApplicantName = a.Applicant.Name,
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

        public async Task<List<ActiveJopPosting>> GetActiveJobPostingsAsync(int hrId)
        {
            return await _JobPostRepository.GetAll()
                .Where(j => j.HRId == hrId && j.JobStatus == enJobStatus.Active)
                .Select(j => new ActiveJopPosting
                {
                    JobTitle = j.Title,
                    ApplicationTotalCount = j.Applications.Count,
                    JobStatus = j.JobStatus,
                    TakenExamCount = j.Applications.Count(a => a.ExamStatus == enExamStatus.completed),
                    JobPostLink = $"/jobopenings/{j.Id}"
                })
                .ToListAsync();
        }

    }
}
