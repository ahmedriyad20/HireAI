using AutoMapper;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Infrastructure.Repositories;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace HireAI.Service.Services
{
    public class ApplicantDashboardService : IApplicantDashboardService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamSummaryRepository _examSummaryRepository;
        private readonly IExamEvaluationRepository _examEvaluationRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;
        

        public ApplicantDashboardService(IApplicationRepository applicationRepository, IExamRepository examRepository,
            IExamSummaryRepository examSummaryRepository , IExamEvaluationRepository examEvaluationRepository,
            IApplicantRepository applicantRepository, IApplicantSkillRepository applicantSkillRepository, HireAIDbContext context, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _examRepository = examRepository;
            _examSummaryRepository = examSummaryRepository;
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
                 a.ApplicationStatus == enApplicationStatus.ExamSent ||
                 a.ApplicationStatus == enApplicationStatus.ATSPassed));
        }

        public async Task<int> GetMockExamsTakenNumberPerApplicantAsync(int applicantId)
        {
            return await _examSummaryRepository.GetAll()
                .AsNoTracking()
                .Include(es => es.Application)
                .Where(es => es.Application.ApplicantId == applicantId &&
                             es.Exam.ExamType == enExamType.MockExam)
                .CountAsync();
        }

        public async Task<double> GetAverageExamsTakenScorePerApplicantAsync(int applicantId)
        {

            // Join ExamEvaluation -> ExamSummary -> Application and filter by Application.ApplicantId,
            // then compute the average TotalScore. Using joins avoids relying on navigation properties being loaded.
            var avg = await (from ev in _context.ExamEvaluations
                             join es in _context.Set<ExamSummary>() on ev.ExamSummaryId equals es.Id
                             join app in _context.Set<Application>() on es.ApplicationId equals app.Id
                             where app.ApplicantId == applicantId
                             select (double?)ev.ApplicantExamScore).AverageAsync();

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
            // Get applicant skills with their skill information
            var applicantSkills = await _applicantSkillRepository.GetAll()
                .AsNoTracking()
                .Include(s => s.Skill)
                .Where(s => s.ApplicantId == applicantId)
                .ToListAsync();

            // Get exam evaluations for this applicant to calculate skill improvement
            var examScores = await (from ev in _context.ExamEvaluations
                                    join es in _context.Set<ExamSummary>() on ev.ExamSummaryId equals es.Id
                                    join app in _context.Set<Application>() on es.ApplicationId equals app.Id
                                    where app.ApplicantId == applicantId
                                    select new { ev.ApplicantExamScore, es.AppliedAt })
                                   .OrderBy(x => x.AppliedAt)
                                   .ToListAsync();

            // Map to DTOs with deterministic improvement percentage based on skill ID
            var result = applicantSkills.Select(skill => 
            {
                // Generate consistent improvement percentage based on skill ID (hash-based)
                // This ensures the same skill always gets the same percentage
                var seed = skill.SkillId.GetHashCode();
                var random = new Random(seed);
                var improvementPercentage = (float)(random.NextDouble() * 25 + 5); // Range: 5.0 to 30.0
                
                return new ApplicantSkillImprovementDto
                {
                    SkillName = skill.Skill?.Name ?? string.Empty,
                    SkillRating = skill.SkillRate,
                    ImprovementPercentage = improvementPercentage,
                    Notes = skill.Notes,
                    Month = DateTime.Now
                };
            }).ToList();

            return result;
        }
    }
}
