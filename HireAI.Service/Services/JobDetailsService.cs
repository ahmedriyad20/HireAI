using AutoMapper;
using HireAI.Data.Helpers.DTOs;
using HireAI.Data.Helpers.Enums;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Repositories;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    /// <summary>
    /// Service for managing job details and applications
    /// Provides endpoints for getting job information, applications, and top applicants
    /// </summary>
    public class JobDetailsService : IJobDetailsService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;

        public JobDetailsService(
            IJobPostRepository jobPostRepository,
            IApplicationRepository applicationRepository,
            IApplicantRepository applicantRepository,
            IMapper mapper)
        {
            _jobPostRepository = jobPostRepository;
            _applicationRepository = applicationRepository;
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all jobs for a specific HR with summary statistics
        /// </summary>
        public async Task<List<HRJobSummaryDto>> GetAllJobsForHRAsync(int hrId)
        {
            var jobs = await _jobPostRepository.GetAll()
                .AsNoTracking()
                .Where(j => j.HRId == hrId)
                .Include(j => j.ExamEvaluations)
                .Include(j => j.Applications)
                    .ThenInclude(a => a.ExamEvaluation)
                        //.ThenInclude(es => es.)
                .ToListAsync();

            var result = new List<HRJobSummaryDto>();

            foreach (var job in jobs)
            {
                var applications = job.Applications ?? new List<HireAI.Data.Models.Application>();
                
                result.Add(new HRJobSummaryDto
                {
                    HRId = hrId,
                    JobId = job.Id,
                    Title = job.Title,
                    CompanyName = job.CompanyName,
                    CreatedAt = job.CreatedAt,
                    JobStatus = job.JobStatus.ToString(),
                    TotalApplications = applications.Count,
                    ApplicantsTookExam = applications.Count(a => a.ExamStatus == enExamStatus.Completed),
                    AvgATSScore = applications.Any(a => a.AtsScore.HasValue)
                        ? (float)applications.Where(a => a.AtsScore.HasValue).Average(a => a.AtsScore)
                        : null,
                    AvgExamScore = applications
                        .Where(a => a.ExamEvaluation != null)
                        .Any()
                        ? (float)applications
                            .Where(a => a.ExamEvaluation != null)
                            .Average(a => a.ExamEvaluation.ApplicantExamScore)
                        : null
                });
            }

            return result;
        }

        /// <summary>
        /// Get detailed information about a job with all applications and top applicants
        /// </summary>
        public async Task<JobDetailsDto> GetJobDetailsAsync(int jobId, int hrId)
        {
            var job = await _jobPostRepository.GetAll()
                .AsNoTracking()
                .Where(j => j.Id == jobId && j.HRId == hrId)
                .Include(j => j.ExamEvaluations)
                .Include(j => j.Applications)
                    .ThenInclude(a => a.Applicant)
                .Include(j => j.Applications)
                    .ThenInclude(a => a.ExamEvaluation)
                .FirstOrDefaultAsync();

            if (job == null)
                throw new KeyNotFoundException($"Job with ID {jobId} not found or does not belong to HR {hrId}");

            var applications = job.Applications ?? new List<HireAI.Data.Models.Application>();

            var jobDetailsDto = new JobDetailsDto
            {
                Id = job.Id,
                Title = job.Title,
                CompanyName = job.CompanyName,
                Description = job.Description,
                CreatedAt = job.CreatedAt,
                UpdatedAt = job.UpdatedAt,
                JobStatus = job.JobStatus,
                ExamDurationMinutes = job.ExamDurationMinutes,
                ExperienceLevel = job.ExperienceLevel,
                EmploymentType = job.EmploymentType,
                Location = job.Location,
                SalaryRange = job.SalaryRange,
                NumberOfQuestions = job.NumberOfQuestions,
                ApplicationDeadline = job.ApplicationDeadline,
                ATSMinimumScore = job.ATSMinimumScore,
                AutoSend = job.AutoSend,
                TotalApplications = applications.Count,
                ApplicantsTookExam = applications.Count(a => a.ExamStatus == enExamStatus.Completed),
                Applications = await GetJobApplicationsAsync(jobId, hrId),
                TopApplicants = await GetTopApplicantsAsync(jobId, hrId, 10),
                TopExamTakers = await GetTopExamTakersAsync(jobId, hrId, 10)
            };

            return jobDetailsDto;
        }

        /// <summary>
        /// Get all applications for a job with applicant details
        /// </summary>
        public async Task<List<JobApplicationDto>> GetJobApplicationsAsync(int jobId, int hrId)
        {
            var applications = await _applicationRepository.GetAll()
                .AsNoTracking()
                .Where(a => a.JobId == jobId && a.HRId == hrId)
                .Include(a => a.Applicant)
                .Include(a => a.ExamEvaluation)
                .ToListAsync();

            return applications.Select(a => new JobApplicationDto
            {
                ApplicationId = a.Id,
                ApplicantId = a.ApplicantId ?? 0,
                ApplicantName = a.Applicant?.FullName ?? "Unknown",
                ApplicantPhone = a.Applicant?.Phone ?? "N/A",
                CVKey = a.CVFilePath ?? string.Empty,
                DateApplied = a.DateApplied,
                ATSScore = a.AtsScore,
                ApplicationStatus = a.ApplicationStatus,
                ExamStatus = a.ExamStatus
            }).ToList();
        }

        /// <summary>
        /// Get top applicants for a job ranked by ATS score
        /// </summary>
        public async Task<List<TopApplicantDto>> GetTopApplicantsAsync(int jobId, int hrId, int topCount = 10)
        {
            var topApplicants = await _applicationRepository.GetAll()
                .AsNoTracking()
                .Where(a => a.JobId == jobId && a.HRId == hrId && a.AtsScore.HasValue)
                .Include(a => a.Applicant)
                .OrderByDescending(a => a.AtsScore)
                .Take(topCount)
                .ToListAsync();

            var result = new List<TopApplicantDto>();
            int rank = 1;

            foreach (var app in topApplicants)
            {
                result.Add(new TopApplicantDto
                {
                    ApplicationId = app.Id,
                    ApplicantId = app.ApplicantId ?? 0,
                    ApplicantName = app.Applicant?.FullName ?? "Unknown",
                    ApplicantPhone = app.Applicant?.Phone ?? "N/A",
                    CVKey = app.CVFilePath ?? string.Empty,
                    ATSScore = app.AtsScore,
                    DateApplied = app.DateApplied,
                    Rank = GetRankString(rank++)
                });
            }

            return result;
        }

        /// <summary>
        /// Get top exam takers for a job ranked by exam score
        /// </summary>
        public async Task<List<TopExamTakerDto>> GetTopExamTakersAsync(int jobId, int hrId, int topCount = 10)
        {
            var topExamTakers = await _applicationRepository.GetAll()
                .AsNoTracking()
                .Where(a => a.JobId == jobId &&
                           a.HRId == hrId &&
                           a.ExamStatus == enExamStatus.Completed &&
                           a.ExamEvaluation != null)
                .Include(a => a.Applicant)
                .Include(a => a.ExamEvaluation)
                .OrderByDescending(a => a.ExamEvaluation.ApplicantExamScore)
                .Take(topCount)
                .ToListAsync();

            var result = new List<TopExamTakerDto>();
            int rank = 1;

            foreach (var app in topExamTakers)
            {
                var examEvaluation = app.ExamEvaluation ?? new HireAI.Data.Models.ExamEvaluation();

                result.Add(new TopExamTakerDto
                {
                    ApplicationId = app.Id,
                    ApplicantId = app.ApplicantId ?? 0,
                    ApplicantName = app.Applicant?.FullName ?? "Unknown",
                    ApplicantPhone = app.Applicant?.Phone ?? "N/A",
                    CVKey = app.CVFilePath ?? string.Empty,
                    DateApplied = app.DateApplied,
                    ExamScore = examEvaluation.ApplicantExamScore,
                    Rank = GetRankString(rank++)
                });
            }

            return result;
        }

        /// <summary>
        /// Convert rank number to ordinal string (1st, 2nd, 3rd, etc.)
        /// </summary>
        private string GetRankString(int rank)
        {
            return rank switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => $"{rank}th"
            };
        }
    }
}
