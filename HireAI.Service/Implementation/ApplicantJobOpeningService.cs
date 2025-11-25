using AutoMapper;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Data.DTOs;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Implementation
{
    public class ApplicantJobOpeningService:IApplicantJobOpeningService
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IMapper _mapper;
        private readonly HireAIDbContext _context;
        public ApplicantJobOpeningService(IMapper mapper,HireAIDbContext context,IJobOpeningRepository JobOpeninRepository)
        {
            _mapper = mapper;
            _context = context;
            _jobOpeningRepository = JobOpeninRepository;
        }
        public async Task<List<JobOpeningDTO>> GetAllJobOpeningAsync()
        {
            var jobopening = await  _jobOpeningRepository.GetAll().ToListAsync();
            var JobOpeningDTO =  _mapper.Map<List<JobOpeningDTO>>(jobopening);
            return  JobOpeningDTO;
        }
    }
}
