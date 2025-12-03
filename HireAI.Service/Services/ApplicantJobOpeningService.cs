using AutoMapper;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenericBase;
using HireAI.Data.DTOs;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
    
using HireAI.Data.Models;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
using HireAI.Data.Helpers.Enums;

namespace HireAI.Service.Services
{
    public class ApplicantJobPostService:IApplicantJobPostService
    {
        private readonly IJobPostRepository _JobPostRepositoryRepository;
        private readonly IMapper _mapper;
        private readonly HireAIDbContext _context;
        public ApplicantJobPostService(IMapper mapper,HireAIDbContext context, IJobPostRepository JobOpeninRepository)
        {
            _mapper = mapper;
            _context = context;
           _JobPostRepositoryRepository = JobOpeninRepository;
        }
        public async Task<List<JobOpeningDTO>> GetAllJobPostAsync()
        {
            var jobPost = await _JobPostRepositoryRepository.GetAll().Where(j => j.JobStatus == enJobStatus.Active).ToListAsync();
            var JobPostDTO = _mapper.Map<List<JobOpeningDTO>>(jobPost);
            return JobPostDTO;
        }
    }
}
