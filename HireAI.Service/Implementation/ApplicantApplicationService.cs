using AutoMapper;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.DTOs;
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
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamEvaluationRepository _examEvaluationRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;

        public ApplicantApplicationService(IJobOpeningRepository jobOpeningRepository, IApplicationRepository applicationRepository, IExamRepository examRepository,
            IExamEvaluationRepository examEvaluationRepository, IApplicantRepository applicantRepository,
            IApplicantSkillRepository applicantSkillRepository, HireAIDbContext context, IMapper mapper)
        {
            _jobOpeningRepository = jobOpeningRepository;
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
