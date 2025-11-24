using AutoMapper;
using HireAI.Data.Models;
using HireAI.Data.DTOs.ApplicantDashboard;

namespace HireAI.Service.Mapping
{
    public class ApplicantSkillProfile : Profile
    {
        public ApplicantSkillProfile()
        {
            CreateMap<ApplicantSkill, ApplicantSkillImprovementDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Title : string.Empty))
                .ForMember(dest => dest.SkillRating, opt => opt.MapFrom(src => src.SkillRate))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImprovementPercentage, opt => opt.MapFrom(src => src.ImprovementPercentage))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}
