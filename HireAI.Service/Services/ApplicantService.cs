using AutoMapper;
using HireAI.Data.Helpers.DTOs.Applicant;
using HireAI.Data.Models;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;

        public ApplicantService(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicantResponseDto>> GetAllApplicantsAsync()
        {
            var applicants = await _applicantRepository.GetAll().AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<ApplicantResponseDto>>(applicants);
        }

        public async Task<ApplicantResponseDto?> GetApplicantByIdAsync(int applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
                return null;
            return _mapper.Map<ApplicantResponseDto>(applicant);
        }

        public async Task<ApplicantResponseDto> AddApplicantAsync(Applicant applicant)
        {
            var createdApplicant = await _applicantRepository.AddAsync(applicant);
            return _mapper.Map<ApplicantResponseDto>(createdApplicant);
        }

        public async Task<ApplicantResponseDto> UpdateApplicantAsync(Applicant applicant)
        {
            var updatedApplicant = await _applicantRepository.UpdateAsync(applicant);
            return _mapper.Map<ApplicantResponseDto>(updatedApplicant);
        }

        public async Task DeleteApplicantAsync(int applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                throw new Exception($"Applicant with ID {applicantId} not found.");
            }
            await _applicantRepository.DeleteAsync(applicant);
        }
    }
}
