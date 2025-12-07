using AutoMapper;
using HireAI.Data.DTOs;
using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Data.Helpers.DTOs.ApplicantApplication;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Helpers.DTOs.ExamResponseDTOS.Request;

using HireAI.Data.Helpers.DTOs.HRDTOS;

using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
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
              .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus.ToString())); // Convert enum to string

            CreateMap<Application, ApplicantApplicationsListDto>()
                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob!.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AppliedJob!.CompanyName))
                .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.AppliedJob!.Location))
                .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.DateApplied))
                .ForMember(dest => dest.AtsScore, opt => opt.MapFrom(src => src.AtsScore))
                .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus.ToString())); // Convert enum to string

            CreateMap<Exam, ExamResponseDTO>();
            CreateMap<Question, QuestionResponseDTO>();
            CreateMap<Answer, AnswerResponseDTO>();


            CreateMap<ExamRequestDTO, Exam>();
            CreateMap<QuestionRequestDTO, Question>();
            CreateMap<AnswerRequestDTO, Answer>();




            CreateMap<JobPostRequestDto, JobPost>();

            // POST / PUT mapping
            CreateMap<JobPostRequestDto, JobPost>();



            CreateMap<HR, HRResponseDto>();

            CreateMap<HRUpdateDto, HR>();

            CreateMap<HRCreateDto, HR>();

            CreateMap<JobPost, JobPostResponseDto>()
                // Usually AutoMapper will map the collection automatically when types match,
                // but you can be explicit:
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.JobSkills));



            CreateMap<Application, ApplicationDetailsDto>()
                 .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Title : string.Empty))
                 .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CompanyName : string.Empty))
                 .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Location : string.Empty))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.CreatedAt : DateTime.UtcNow))
                 .ForMember(dest => dest.IntrviewDate, opt => opt.Ignore()) // Set this manually or add to Application model
                 .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.ExperienceLevel : enExperienceLevel.EntryLevel))
                 .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.SalaryRange : string.Empty))
                 .ForMember(dest => dest.ExamScore, opt => opt.MapFrom(src => src.ExamSummary != null ? (int?)src.ExamSummary.ApplicantExamScore : null))
                 .ForMember(dest => dest.NumberOfApplicants, opt => opt.Ignore())
                 .ForMember(dest => dest.AtsScore, opt => opt.MapFrom(src => src.AtsScore))
                 .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus))
                 .ForMember(dest => dest.ExamEvaluationStatus, opt => opt.MapFrom(src => src.ExamSummary != null && src.ExamSummary.ExamEvaluation != null ? src.ExamSummary.ExamEvaluation.Status : enExamEvaluationStatus.Pending));

            // Add mapping for ApplicantSkill to ApplicantSkillImprovementDto
            CreateMap<ApplicantSkill, ApplicantSkillImprovementDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : string.Empty))
                .ForMember(dest => dest.SkillRating, opt => opt.MapFrom(src => src.SkillRate))
                .ForMember(dest => dest.ImprovementPercentage, opt => opt.MapFrom(src => src.ImprovementPercentage))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => DateTime.UtcNow)); // Or use a timestamp field if available
        }
    }
}

