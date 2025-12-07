using AutoMapper;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Data.Helpers.DTOs.Applicant;
using HireAI.Data.Models;

namespace HireAI.Service.Mapping
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            // Add mapping for ApplicantSkill to ApplicantSkillImprovementDto
            CreateMap<ApplicantSkill, ApplicantSkillImprovementDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : string.Empty))
                .ForMember(dest => dest.SkillRating, opt => opt.MapFrom(src => src.SkillRate))
                .ForMember(dest => dest.ImprovementPercentage, opt => opt.MapFrom(src => src.ImprovementPercentage))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Add mapping for ApplicantSkill to ApplicantSkillDto
            CreateMap<ApplicantSkill, ApplicantSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : string.Empty));


            CreateMap<Applicant, ApplicantResponseDto>();
        }
    }
}
