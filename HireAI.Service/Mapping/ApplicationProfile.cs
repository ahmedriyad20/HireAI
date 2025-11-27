using AutoMapper;
using HireAI.Data.DTOs;
using HireAI.Data.DTOs.ApplicantDashboard;

using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Helpers.DTOs.ExamResponseDTOS.Request;

using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpening.ResonsetDto;
using HireAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
