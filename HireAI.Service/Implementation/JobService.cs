using AutoMapper;
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{

    public class JobService: IJopService

    {
        private readonly IMapper _mapper;
        private readonly IJobPostRepository _jobPostRepository;

        public JobService(IMapper mapper , IJobPostRepository jobOpeningRepository)
        {
            _mapper = mapper;
            _jobPostRepository = jobOpeningRepository;
        }

        public async Task CreateJobPostAsync(JobPostRequestDto jopOpeingRequestDto)
        {
            var createPostEntity = _mapper.Map<JobPost>(jopOpeingRequestDto);
            await _jobPostRepository.AddAsync(createPostEntity);
        }

  
        public async Task DeleteJobPostAsync(int id)
        {
            var jobOpeningEntity = await _jobPostRepository.GetByIdAsync(id);
            if (jobOpeningEntity == null)
            {
                throw new Exception("Job Opening not found");
            }
            await _jobPostRepository.DeleteAsync(jobOpeningEntity);
        }

        public Task<JobPostResponseDto> GetJobPostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<JobPostResponseDto>> GetJobPostForHrAsync(int hrid)
        {
            var jobOpenings = await _jobPostRepository.GetJobPostForHrAsync(hrid);

            if (jobOpenings == null || !jobOpenings.Any())
            {
                return Array.Empty<JobPostResponseDto>();
            }
            return _mapper.Map<ICollection<JobPostResponseDto>>(jobOpenings);
        }

        public async Task UpdateJobPostAsync(int id, JobPostRequestDto jopOpeingRequestDto)
        {
            var existingEntity = await _jobPostRepository.GetByIdAsync(id);

            if (existingEntity == null)
                throw new Exception("Job Opening not found");


            _mapper.Map(jopOpeingRequestDto, existingEntity);

            await _jobPostRepository.UpdateAsync(existingEntity);
        }
    }
}
      
 
       
     



