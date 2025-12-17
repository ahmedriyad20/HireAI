using HireAI.Data.Helpers.DTOs.Applicant.response;
using HireAI.Data.Helpers.DTOs.ReportDtos.resposnes;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IApplicationRepository _applicantRepository; 
        public ReportService(IJobPostRepository jobPostRepository , IApplicationRepository applicationRepository) {
            _jobPostRepository = jobPostRepository;
            _applicantRepository = applicationRepository; 
        }
        public async Task<ReportDto> GetReportByJobIdAsync(int jobId)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(jobId); // fetch job once

            if (jobPost == null)
                throw new Exception("Job not found");

            var applicantDtos = await _jobPostRepository.GetApplicantDtosForJobAsync(jobId);

            var totalApplicants = await _jobPostRepository.GetTotalApplicationAsncy(jobId);
            var atsPassingScore = await _applicantRepository.GetAtsPassingScore(jobId);
            var atsPassPercent = totalApplicants == 0 ? 0 : (float)atsPassingScore / totalApplicants * 100;

            var report = new ReportDto
            {
                JobTitle = jobPost.Title,
                TotalApplicants = totalApplicants,
                AtsPassPercent = atsPassPercent,
                Applicants = applicantDtos.ToList()
            };

            return report;
        }

    }
}

