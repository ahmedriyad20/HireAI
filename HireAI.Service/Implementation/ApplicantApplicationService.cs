using AutoMapper;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Data.DTOs.ApplicantDashboard;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Implementation
{
    public class ApplicantApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicantApplicationService(IApplicationRepository applicationRepository, IMapper mapper)
        {
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
    }
}
