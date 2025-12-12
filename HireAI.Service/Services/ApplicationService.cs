using AutoMapper;
using AutoMapper.QueryableExtensions;
using HireAI.Data.Helpers.DTOs.Application;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly HireAIDbContext _context;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository applicationRepository, HireAIDbContext context, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetAllApplicationsAsync()
        {
            var applications = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.AppliedJob)
                .Include(a => a.HR)
                .ProjectTo<ApplicationResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return applications;
        }

        public async Task<ApplicationResponseDto?> GetApplicationByIdAsync(int applicationId)
        {
            var application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.AppliedJob)
                .Include(a => a.HR)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            return _mapper.Map<ApplicationResponseDto>(application);
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByApplicantIdAsync(int applicantId)
        {
            var applications = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.AppliedJob)
                .Include(a => a.HR)
                .Where(a => a.ApplicantId == applicantId)
                .ProjectTo<ApplicationResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return applications;
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByJobIdAsync(int jobId)
        {
            var applications = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.AppliedJob)
                .Include(a => a.HR)
                .Where(a => a.JobId == jobId)
                .ProjectTo<ApplicationResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return applications;
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByHRIdAsync(int hrId)
        {
            var applications = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.AppliedJob)
                .Include(a => a.HR)
                .Where(a => a.HRId == hrId)
                .ProjectTo<ApplicationResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return applications;
        }

        public async Task<ApplicationResponseDto> CreateApplicationAsync(CreateApplicationDto createDto)
        {
            var application = _mapper.Map<Application>(createDto);
            application.DateApplied = DateTime.Now;

            await _applicationRepository.AddAsync(application);

            return await GetApplicationByIdAsync(application.Id) 
                ?? throw new InvalidOperationException("Failed to create application");
        }

        public async Task<ApplicationResponseDto> UpdateApplicationAsync(UpdateApplicationDto updateDto)
        {
            var application = await _applicationRepository.GetByIdAsync(updateDto.Id);

            if (application == null)
            {
                throw new KeyNotFoundException($"Application with ID {updateDto.Id} not found");
            }

            // Map only the properties from UpdateDto to existing entity
            _mapper.Map(updateDto, application);

            await _applicationRepository.UpdateAsync(application);

            return await GetApplicationByIdAsync(application.Id) 
                ?? throw new InvalidOperationException("Failed to update application");
        }

        public async Task<bool> DeleteApplicationAsync(int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return false;
            }

            await _applicationRepository.DeleteAsync(application);

            return true;
        }
    }
}