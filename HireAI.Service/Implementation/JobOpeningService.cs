using AutoMapper;
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
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
    internal class JobOpeningService: IJopOpenningService
    {
        private readonly IMapper _mapper;
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public JobOpeningService(IMapper mapper , IJobOpeningRepository jobOpeningRepository)
        {
            _mapper = mapper;
            _jobOpeningRepository = jobOpeningRepository;
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
