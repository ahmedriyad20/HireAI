using AutoMapper;
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpening.ResonsetDto;
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

    public class JobOpeningService: IJopOpenningService

    {
        private readonly IMapper _mapper;
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public JobOpeningService(IMapper mapper , IJobOpeningRepository jobOpeningRepository)
        {
            _mapper = mapper;
            _jobOpeningRepository = jobOpeningRepository;
        }

        public async Task CreateJopOppenAsny(JopOpeingRequestDto jopOpeingRequestDto)
        {
            var createJobOpeningEntity = _mapper.Map<JobOpening>(jopOpeingRequestDto);
            await _jobOpeningRepository.AddAsync(createJobOpeningEntity);
        }

        public async Task DeleteJopOppenAsny(int id)
        {
            var jobOpeningEntity = await _jobOpeningRepository.GetByIdAsync(id);
            if (jobOpeningEntity == null)
            {
                throw new Exception("Job Opening not found");
            }
            await _jobOpeningRepository.DeleteAsync(jobOpeningEntity);
        }
        public async Task<ICollection<JobOpeningResponseDto>> GetJopOpeningForHrAsync(int hrid)
        {
            var jobOpenings = await _jobOpeningRepository.GetJobOpeningForHrAsync(hrid);

            if (jobOpenings == null || !jobOpenings.Any())
            {
                return Array.Empty<JobOpeningResponseDto>();
            }

            // Map entities to response DTO
            return _mapper.Map<ICollection<JobOpeningResponseDto>>(jobOpenings);
        }
        public async Task UpdateJopOppenAsny(int id, JopOpeingRequestDto jopOpeingRequestDto)
        {
            var existingEntity = await _jobOpeningRepository.GetByIdAsync(id);

            if (existingEntity == null)
                throw new Exception("Job Opening not found");


            _mapper.Map(jopOpeingRequestDto, existingEntity);

            await _jobOpeningRepository.UpdateAsync(existingEntity);
        }
    

     

        public async Task AddJopOppenAsny(CreateJopOpeingRequestDto jopOpeingRequestDto)
        {
           await _jobOpeningRepository.AddAsync(CreateJopOpeningEntity(jopOpeingRequestDto));
        }

        private JobOpening CreateJopOpeningEntity(CreateJopOpeingRequestDto jopOpeingRequestDto)
        {
            return _mapper.Map<JobOpening>(jopOpeingRequestDto);
        }
    }
}
      
 
       
     



