using AutoMapper;
using HireAI.Data.Helpers.DTOs.Exam;
using HireAI.Data.Models;

namespace HireAI.Service.Mapping
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, MockExamDto>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.ExamName))
                .ForMember(dest => dest.ExamDescription, opt => opt.MapFrom(src => src.ExamDescription))
                .ForMember(dest => dest.ExamLevel, opt => opt.MapFrom(src => src.ExamLevel))
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.NumberOfQuestions))
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.DurationInMinutes));
        }
    }
}
