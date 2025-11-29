using AutoMapper;
using HireAI.Data.DTOs;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Data.Helpers.DTOs.ApplicantApplication;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Helpers.DTOs.ExamResponseDTOS.Request;
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpening.ResonsetDto;
using HireAI.Data.Models;

namespace HireAI.Infrastructure.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationTimelineItemDto>()
              .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Title : string.Empty))
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CompanyName : string.Empty))
              .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.DateApplied))
              .ForMember(dest => dest.AtsScore, opt => opt.MapFrom(src => src.AtsScore))
              .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus));

            CreateMap<Exam, ExamResponseDTO>();
            CreateMap<Question, QuestionResponseDTO>();
            CreateMap<Answer, AnswerResponseDTO>();


            CreateMap< ExamRequestDTO , Exam>();
            CreateMap< QuestionRequestDTO , Question>();
            CreateMap<AnswerRequestDTO, Answer>();


            CreateMap<JobOpening, JobOpeningDTO>();

            CreateMap<JopOpeingRequestDto, JobOpening>();

            // POST / PUT mapping
            CreateMap<CreateJopOpeingRequestDto, JobOpening>();

            // GET mapping (response)
            CreateMap<JobOpening, JobOpeningResponseDto>();

            // Optional: if you have other DTOs
            CreateMap<JobOpening, JobOpeningDTO>();

            CreateMap<Application, ApplicationDetailsDto>()
                 .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Title : string.Empty))
                 .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CompanyName : string.Empty))
                 .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Location : string.Empty))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CreatedAt : DateTime.UtcNow))
                 .ForMember(dest => dest.NumberOfApplicants, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Applications.Count : 0))
                 .ForMember(dest => dest.AtsScore, opt => opt.MapFrom(src => src.AtsScore))
                 .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus))
                 .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.ExamSummary != null && src.ExamSummary.ExamEvaluation != null ? src.ExamSummary.ExamEvaluation.IsPassed : false))
                 .ForMember(dest => dest.ExamEvaluationStatus, opt => opt.MapFrom(src => src.ExamSummary != null && src.ExamSummary.ExamEvaluation != null ? src.ExamSummary.ExamEvaluation.Status : enExamEvaluationStatus.Pending));
        }
    }
}
