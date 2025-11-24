using HireAI.Data.Helpers.DTOs.Respones;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{
    public class HRDashBoardService : IHrDashboardService
    {
        private readonly IApplicationRepository _applications;

        private readonly IJobOpeningRepository _jobOpening;
        public HRDashBoardService(IJobOpeningRepository jobOpeningRepository, IApplicationRepository applications)
        {
            _applications = applications;
 

            _jobOpening = jobOpeningRepository;
        }
        public async Task<HRDashboardDto> GetDashboardAsync(int hrId)
        {
      

            return new HRDashboardDto
            {
                TotalApplicants = await GetToTalApplicantsAsync(hrId).ConfigureAwait(false),
                TotalExamTaken = await GetTotalExamTakenAsync(hrId),
                TotalTopCandidates = await GetTotalTopCandidatesAsync(hrId),
                MonthlyApplicants = await GetMonthlyNumberOfApplicationsAsync(hrId),
                ATSPassedRateMonthly = await GetMonthlyOfTotalATSPassedAsync(hrId),
                RecentApplications = await GetRecentApplicantsAsync(hrId),
                ActiveJopPostings = await GetActiveJobPostingsAsync(hrId)

            };
        }
        private  async Task<int> GetToTalApplicantsAsync(int hrId)
        {
            var query =    _applications.GetAll();

            var filtered = query.Where(a => a.HRId == hrId);

            return await filtered.CountAsync();
        }

        private async Task<int> GetTotalExamTakenAsync(int hrId)
        {
            var query =  _applications.GetAll(); 
            var filtered = query.Where(a => a.HRId == hrId && a.ExamStatus == enExamStatus.completed); 
             return await filtered.CountAsync(); 
           
        }

        private async Task<int> GetTotalTopCandidatesAsync(int hrId)
        {
            var applicationQuery =  _applications.GetAll(); //no sql yet
            return await applicationQuery.Where(a => a.ExamSummary != null && a.ExamSummary.TotalScroe <80 && a.AtsScore < 80).CountAsync();
        }

        private async Task<Dictionary<int, int>> GetMonthlyNumberOfApplicationsAsync(int hrId)
        {
            var query =  _applications.GetAll(); //no sql yet
            return await query
                .Where(a => a.HRId == hrId && a.DateApplied > DateTime.UtcNow.AddYears(-1))
                .GroupBy(a => a.DateApplied.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
               .ToDictionaryAsync(x => x.Month, x => x.Count);
        }

        private async Task<Dictionary<int, int>> GetMonthlyOfTotalATSPassedAsync(int hrId)
        {
            var query =  _applications.GetAll(); 
            return await query
                .Where(a => a.HRId == hrId && a.ApplicationStatus == enApplicationStatus.UnderReview)
                .GroupBy(a => a.DateApplied.Month)
                .Select(a => new { Month = a.Key, Count = a.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);
        }

        private async Task<List<RecentApplicationDto>> GetRecentApplicantsAsync(int hrId, int take = 5)
        {
            var query =  _applications.GetAll();
            return await
                query.Where(a => a.HRId == hrId).Select
                (a => new RecentApplicationDto
                {
                    ApplicantName = a.Applicant.Name,
                    ApplicantCVlink = a.CVFilePath,
                    AppliedOn = a.DateApplied,
                    ATSScore = a.AtsScore ?? 0,
                    Position = a.AppliedJob.Title,
                    JobStatus = a.AppliedJob.JobStatus,
                    ExamResultLink = a.ExamSummary != null ? $"/examsummary/{a.ExamSummary.Id}" : " "

                }).Take(take).ToListAsync();

        }

        private async Task<List<ActiveJopPosting>> GetActiveJobPostingsAsync(int hrId)
        {
            var query = _jobOpening.GetAll();
            return await 
                query.Where(a => a.HRId == hrId && a.JobStatus == enJobStatus.Active).Select
                (a => new ActiveJopPosting
                {
                    JobTitle = a.Title,
                    ApplicationTotalCount = a.Applications.Count , 
                    JobStatus = a.JobStatus,

                    TakenExamCount = a.Applications.Where(app => app.ExamStatus == enExamStatus.completed).Count(),


                    JobPostLink = $"/jobopenings/{a.Id}"

                }).Distinct().ToListAsync();
        }
    }
}
