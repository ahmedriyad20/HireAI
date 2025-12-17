using AutoMapper;
using HireAI.Data.Helpers.DTOs.Exam;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Services
{
    public class MockExamService : IMockExamService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IExamRepository _examRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IExamSummaryRepository _examSummaryRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;

        public MockExamService(IApplicationRepository applicationRepository, IExamRepository examRepository,
             IApplicantRepository applicantRepository, IExamSummaryRepository examSummaryRepository, HireAIDbContext context, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _examRepository = examRepository;
            _applicantRepository = applicantRepository;
            _examSummaryRepository = examSummaryRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> GetMockExamsTakenNumberPerApplicantAsync(int applicantId)
        {
            return await _examSummaryRepository.GetAll()
                .AsNoTracking()
                .Include(es => es.Applicant)
                .Where(es => es.ApplicantId == applicantId &&
                             es.Exam.ExamType == enExamType.MockExam)
                .CountAsync();
        }

        public async Task<int> GetMockExamsTakenNumberForCurrentMonthPerApplicantAsync(int applicantId)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return await _examSummaryRepository.GetAll()
                .AsNoTracking()
                .Include(es => es.Applicant)
                .Include(es => es.Exam)
                .Where(es => es.ApplicantId == applicantId &&
                             es.Exam.ExamType == enExamType.MockExam &&
                             es.AppliedAt.Month == currentMonth &&
                             es.AppliedAt.Year == currentYear)
                .CountAsync();
        }

        public async Task<double> GetAverageExamsTakenScorePerApplicantAsync(int applicantId)
        {

            // Join ExamEvaluation -> ExamSummary -> Application and filter by Application.ApplicantId,
            // then compute the average TotalScore. Using joins avoids relying on navigation properties being loaded.
            var avg = await (from ev in _context.ExamEvaluations
                             join es in _context.Set<ExamSummary>() on ev.ExamSummaryId equals es.Id
                             join app in _context.Set<Applicant>() on es.ApplicantId equals app.Id
                             where app.Id == applicantId
                             select (double?)ev.ApplicantExamScore).AverageAsync();

            if (avg == null)
                return 0.0;

            // Get the first digit after the decimal point
            return Math.Round(avg.Value, 2);
        }

        public async Task<double> GetAverageExamsTakenScoreImprovementPerApplicantAsync(int applicantId)
        {
            var scores = await (from ev in _context.ExamEvaluations
                                join es in _context.Set<ExamSummary>() on ev.ExamSummaryId equals es.Id
                                join app in _context.Set<Applicant>() on es.ApplicantId equals app.Id
                                where app.Id == applicantId
                                orderby es.AppliedAt
                                select ev.ApplicantExamScore).ToListAsync();

            if (scores.Count < 2)
                return 0.0;

            // Calculate improvement from first exam to last exam
            return scores.Last() - scores.First();
        }

        public async Task<IEnumerable<MockExamDto>> GetRecommendedMockExamsPerApplicantAsync(int applicantId)
        {
            var applicant = await _applicantRepository.GetAll()
                .AsNoTracking()
                .Include(a => a.ApplicantSkills)
                .ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (applicant == null)
            {
                throw new KeyNotFoundException("Applicant not found.");
            }

            // Get applicant's skill titles (normalized for comparison)
            var applicantSkillTitles = applicant.ApplicantSkills?
                .Where(s => s.Skill != null)
                .Select(s => s.Skill!.Name.ToLower().Trim())
                .ToHashSet() ?? new HashSet<string>();

            // Get all mock exams
            var allMockExams = await _examRepository.GetAll()
                .AsNoTracking()
                .Where(e => e.ExamType == enExamType.MockExam && e.ExamName != null)
                .ToListAsync();

            // Match exams where exam name contains any applicant skill
            var matchedExams = allMockExams
                .Where(e => applicantSkillTitles.Any(skill => e.ExamName.ToLower().Contains(skill)))
                .OrderBy(e => e.ExamLevel)
                .Take(6)
                .Select(e => _mapper.Map<MockExamDto>(e))
                .ToList();

            // If fewer than 3 matched exams, fill with additional exams by level
            if (matchedExams.Count < 3)
            {
                int skillCount = applicant.ApplicantSkills?.Count ?? 0;
                enExamLevel recommendedLevel = skillCount switch
                {
                    <= 2 => enExamLevel.Beginner,
                    <= 4 => enExamLevel.Intermediate,
                    <= 6 => enExamLevel.Advanced,
                    _ => enExamLevel.Beginner,
                };

                var additionalExams = allMockExams
                    .Where(e => e.ExamLevel == recommendedLevel &&
                                !matchedExams.Any(me => me.ExamName == e.ExamName))
                    .OrderBy(e => e.CreatedAt)
                    .Take(6 - matchedExams.Count)
                    .Select(e => _mapper.Map<MockExamDto>(e))
                    .ToList();

                matchedExams.AddRange(additionalExams);
            }

            return matchedExams;
        }

        public async Task<IEnumerable<MockExamDto>> GetAllMockExamsPerApplicantAsync(int applicantId)
        {
            var applicant = await _applicantRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (applicant == null)
            {
                throw new KeyNotFoundException("Applicant not found.");
            }

            // Get all mock exams and return 9 random ones
            var randomExams = await _examRepository.GetAll()
                .AsNoTracking()
                .Where(e => e.ExamType == enExamType.MockExam)
                .OrderBy(e => Guid.NewGuid())
                .Select(e => _mapper.Map<MockExamDto>(e))
                .ToListAsync();

            return randomExams;
        }
    }
}
