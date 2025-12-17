using AutoMapper;
using HireAI.Data.Helpers.DTOs.SkillDtos;
using HireAI.Data.Models;

namespace HireAI.Service.Mapping
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, SkillResponseDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        }
    }
}