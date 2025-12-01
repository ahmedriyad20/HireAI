using AutoMapper;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;
using HireAI.Data.Helpers.DTOs.ApplicantApplication;
using HireAI.Service.Interfaces;

namespace HireAI.Service.Implementation
{
    public class ApplicantApplicationService : IApplicantApplicationService
    {
        private readonly IJobPostRepository _jobPostRepository;

        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicantApplicationService(IJobPostRepository jobOpeningRepository, IApplicationRepository applicationRepository, IExamRepository examRepository,
            IExamEvaluationRepository examEvaluationRepository, IApplicantRepository applicantRepository,
            IApplicantSkillRepository applicantSkillRepository, HireAIDbContext context, IMapper mapper)
        {
            _jobPostRepository = jobOpeningRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicantApplicationsListDto>> GetApplicantApplicationsList(int applicantId)
        {
            var ApplicationsList = await _applicationRepository.GetAll()
                .AsNoTracking()
                .Include(a => a.AppliedJob)
                .Where(a => a.ApplicantId == applicantId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ApplicantApplicationsListDto>>(ApplicationsList);
        }

        public async Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int applicationId, int applicantId)
        {
            var application = await _applicationRepository.GetAll()
                .AsNoTracking()
                .Include(a => a.AppliedJob)
                .Include(a => a.ExamSummary)
                    .ThenInclude(e => e.ExamEvaluation)
                .Where(a => a.Id == applicationId && a.ApplicantId == applicantId)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                throw new KeyNotFoundException("Application not found.");
            }

            var dto = _mapper.Map<ApplicationDetailsDto>(application);

            // Get the accurate count using a separate query (prevents Cartesian explosion)
            if (application.JobId.HasValue)
            {
                dto.NumberOfApplicants = await _applicationRepository.GetAll()
                    .AsNoTracking()
                    .CountAsync(a => a.JobId == application.JobId);
            }

            return dto;
        }
    }
}
