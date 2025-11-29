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

namespace HireAI.Service.Implementation
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
        public async Task<List<JobPostResponseDto>> GetAllJobPostAsync()
        {
            var jobPost = await _JobPostRepositoryRepository.GetAll().ToListAsync();
            var JobPostDTO = _mapper.Map<List<JobPostResponseDto>>(jobPost);
            return JobPostDTO;
        }
    }
}
