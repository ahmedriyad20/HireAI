using AutoMapper;
using HireAI.Data.Models;
using HireAI.Data.DTOs.ApplicantDashboard;

namespace HireAI.Service.Mapping
{
    public class AppliedJobsProfile : Profile
    {
        public AppliedJobsProfile()
        {
            CreateMap<Application, ApplicantApplicationsListDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Title : string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CompanyName : string.Empty))
                .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Location : string.Empty))
                .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.DateApplied))
                .ForMember(dest => dest.AtsScore, opt => opt.MapFrom(src => src.AtsScore))
                .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus));
        }
    }
}
