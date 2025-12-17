using HireAI.Data.Helpers.DTOs.SkillDtos;

namespace HireAI.Service.Interfaces
{
    public interface ISkillService
    {
        Task<ICollection<SkillResponseDto>> GetAllSkillsAsync();
        Task<SkillResponseDto?> GetSkillByIdAsync(int id);
        Task<SkillResponseDto> CreateSkillAsync(SkillRequestDto skillRequestDto);
        Task<SkillResponseDto?> UpdateSkillAsync(int id, SkillRequestDto skillRequestDto);
        Task<bool> DeleteSkillAsync(int id);
    }
}