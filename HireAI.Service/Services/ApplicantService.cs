using AutoMapper;
using HireAI.Data.Helpers.DTOs.Applicant;
using HireAI.Data.Helpers.DTOs.Applicant.Request;
using HireAI.Data.Models;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;

        public ApplicantService(IApplicantRepository applicantRepository, IApplicantSkillRepository applicantSkillRepository,
            IMapper mapper, IS3Service s3Service)
        {
            _applicantRepository = applicantRepository;
            _applicantSkillRepository = applicantSkillRepository;
            _mapper = mapper;
            _s3Service = s3Service;
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

        public async Task<ApplicantResponseDto> UpdateApplicantAsync(ApplicantUpdateDto applicantDto)
        {
            var applicant = _mapper.Map<Applicant>(applicantDto);

            // Handle file upload separately
            //if (applicantDto.CvFile != null)
            //{
            //    applicant.ResumeUrl = await _s3Service.UploadFileAsync(applicantDto.CvFile);
            //}
            //else
            //{
            //    var existingApplicant = await _applicantRepository.GetByIdAsync(applicantDto.Id);
            //    if (existingApplicant != null)
            //    {
            //        applicant.ResumeUrl = existingApplicant.ResumeUrl;
            //    }
            //}

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

        public async Task<IEnumerable<ApplicantSkillDto>> AddSkillsToApplicantAsync(int applicantId, List<int> skillIds)
        {
            // Verify applicant exists
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                throw new Exception($"Applicant with ID {applicantId} not found.");
            }

            var addedSkills = new List<ApplicantSkill>();

            foreach (var skillId in skillIds)
            {
                // Check if skill already exists for this applicant
                var existingSkill = applicant.ApplicantSkills
                    .FirstOrDefault(s => s.SkillId == skillId);

                if (existingSkill == null)
                {
                    var applicantSkill = new ApplicantSkill
                    {
                        ApplicantId = applicantId,
                        SkillId = skillId
                    };

                    var createdSkill = await _applicantSkillRepository.AddAsync(applicantSkill);
                    addedSkills.Add(createdSkill);
                }
            }

            return _mapper.Map<IEnumerable<ApplicantSkillDto>>(addedSkills);
        }
    }
}
