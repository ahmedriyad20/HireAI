using AutoMapper;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Abstractions;
using HireAI.Service.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{
    public class ApplicantDashboardService : IApplicantDashboardService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamEvaluationRepository _examEvaluationRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;

        public ApplicantDashboardService(IApplicationRepository applicationRepository, IExamRepository examRepository,
            IExamEvaluationRepository examEvaluationRepository, IApplicantRepository applicantRepository,
            IApplicantSkillRepository applicantSkillRepository, HireAIDbContext context, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _examRepository = examRepository;
            _examEvaluationRepository = examEvaluationRepository;
            _applicantRepository = applicantRepository;
            _applicantSkillRepository = applicantSkillRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> GetActiveApplicationsNumberPerApplicantAsync(int applicantId)
        {
            return await _applicationRepository.GetAll()
            .CountAsync(a =>
                a.ApplicantId == applicantId &&
                (a.ApplicationStatus == enApplicationStatus.UnderReview ||
                 a.ApplicationStatus == enApplicationStatus.UnderReview));
        }

        public async Task<int> GetMockExamsTakenNumberPerApplicantAsync(int applicantId)
        {
            return await _examRepository.GetAll().CountAsync(e =>
            e.ApplicantId == applicantId && e.ExamType == enExamType.MockExam);
        }

        public async Task<double> GetAverageExamsTakenScorePerApplicantAsync(int applicantId)
        {

            // Join ExamEvaluation -> ExamSummary -> Application and filter by Application.ApplicantId,
            // then compute the average TotalScore. Using joins avoids relying on navigation properties being loaded.
            var avg = await (from ev in _context.ExamEvaluations
                             join es in _context.Set<ExamSummary>() on ev.ExamSummaryId equals es.Id
                             join app in _context.Set<Application>() on es.ApplicationId equals app.Id
                             where app.ApplicantId == applicantId
                             select (double?)ev.TotalScore).AverageAsync();

            return avg ?? 0.0;
        }

        public async Task<string> GetSkillLevelPerApplicantAsync(int applicantId)
        {
            var applicant = await _applicantRepository.GetAll()
                .AsNoTracking()
                .Include(a => a.ApplicantSkills).FirstOrDefaultAsync(a => a.Id == applicantId);

            int numberOfSkills = applicant?.ApplicantSkills?.Count ?? 0;

            if (numberOfSkills <= 2) return enSkillLevel.Beginner.ToString();
            if (numberOfSkills <= 4) return enSkillLevel.Intermediate.ToString();
            if (numberOfSkills <= 6) return enSkillLevel.Advanced.ToString();

            return enSkillLevel.Expert.ToString();
        }

        public async Task<IEnumerable<ApplicationTimelineItemDto>> GetApplicationTimelinePerApplicantAsync(int applicantId)
        {
            var Applications = await _applicationRepository.GetAll().Include(a => a.AppliedJob).Where(a => a.ApplicantId == applicantId).ToListAsync();

            var ApplicationsTimeline = _mapper.Map<IEnumerable<ApplicationTimelineItemDto>>(Applications);

            return ApplicationsTimeline;
        }

        public async Task<IEnumerable<ApplicantSkillImprovementDto>> GetApplicantSkillImprovementScoreAsync(int applicantId)
        {
            var applicantSkills = await _applicantSkillRepository.GetAll()
                .AsNoTracking()
                .Include(s => s.Skill)
                .Where(s => s.ApplicantId == applicantId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ApplicantSkillImprovementDto>>(applicantSkills);
        }
    }
}
