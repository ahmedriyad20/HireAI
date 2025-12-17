using AutoMapper;
using HireAI.Data.Helpers.DTOs.SkillDtos;
using HireAI.Data.Models;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IMapper _mapper;

        public SkillService(ISkillRepository skillRepository, IMapper mapper)
        {
            _skillRepository = skillRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<SkillResponseDto>> GetAllSkillsAsync()
        {
            var skills = await _skillRepository.GetAll().ToListAsync();
            return _mapper.Map<ICollection<SkillResponseDto>>(skills);
        }

        public async Task<SkillResponseDto?> GetSkillByIdAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);
            return skill == null ? null : _mapper.Map<SkillResponseDto>(skill);
        }

        public async Task<SkillResponseDto> CreateSkillAsync(SkillRequestDto skillRequestDto)
        {
            var skill = _mapper.Map<Skill>(skillRequestDto);
            var createdSkill = await _skillRepository.AddAsync(skill);
            return _mapper.Map<SkillResponseDto>(createdSkill);
        }

        public async Task<SkillResponseDto?> UpdateSkillAsync(int id, SkillRequestDto skillRequestDto)
        {
            var existingSkill = await _skillRepository.GetByIdAsync(id);
            if (existingSkill == null)
                return null;

            _mapper.Map(skillRequestDto, existingSkill);
            var updatedSkill = await _skillRepository.UpdateAsync(existingSkill);
            return _mapper.Map<SkillResponseDto>(updatedSkill);
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);
            if (skill == null)
                return false;

            await _skillRepository.DeleteAsync(skill);
            return true;
        }
    }
}