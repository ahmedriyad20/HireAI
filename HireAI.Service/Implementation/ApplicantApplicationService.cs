using AutoMapper;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{
    public class ApplicantApplicationService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamEvaluationRepository _examEvaluationRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;

        public ApplicantApplicationService(IJobPostRepository jobOpeningRepository, IApplicationRepository applicationRepository, IExamRepository examRepository,
            IExamEvaluationRepository examEvaluationRepository, IApplicantRepository applicantRepository,
            IApplicantSkillRepository applicantSkillRepository, HireAIDbContext context, IMapper mapper)
        {
            _jobPostRepository = jobOpeningRepository;
            _applicationRepository = applicationRepository;
            _examRepository = examRepository;
            _examEvaluationRepository = examEvaluationRepository;
            _applicantRepository = applicantRepository;
            _applicantSkillRepository = applicantSkillRepository;
            _context = context;
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
    }
}
