using AutoMapper;
using HireAI.Data.Helpers.DTOs.JobPostDtos;
using HireAI.Data.Helpers.DTOs.JobOpening.Request;
using HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response;
using HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response;
using HireAI.Data.Helpers.DTOs.SkillDtos;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Data.DTOs;

namespace HireAI.Service.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between JobPost DTOs and JobPost model.
    /// Handles creation, update, and response mappings.
    /// </summary>
    public class JobPostProfile : Profile
    {
        public JobPostProfile()
        {
            // JobPostRequestDto -> JobPost (Create)
            CreateMap<JobPostRequestDto, JobPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName ?? string.Empty))
                .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.JobStatus ?? enJobStatus.Active))
                .ForMember(dest => dest.JobSkills, opt => opt.Ignore())
                .ForMember(dest => dest.Applications, opt => opt.Ignore())
                .ForMember(dest => dest.ExamEvaluations, opt => opt.Ignore())
                .ForMember(dest => dest.Applicants, opt => opt.Ignore())
                .ForMember(dest => dest.HR, opt => opt.Ignore());

            // ==================== UPDATE MAPPINGS ====================
            // JobPostUpdateDto -> JobPost (Update Operation)
            CreateMap<JobPostUpdateDto, JobPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.JobSkills, opt => opt.Ignore())
                .ForMember(dest => dest.Applications, opt => opt.Ignore())
                .ForMember(dest => dest.ExamEvaluations, opt => opt.Ignore())
                .ForMember(dest => dest.Applicants, opt => opt.Ignore())
                .ForMember(dest => dest.HR, opt => opt.Ignore())
                .ForMember(dest => dest.HRId, opt => opt.Ignore())
                // Only map non-null values
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ==================== RESPONSE MAPPINGS ====================
            //JobPost->JobPostResponseDto(Read Operation)
            CreateMap<JobPost, JobPostResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.JobStatus))
                .ForMember(dest => dest.ExamDurationMinutes, opt => opt.MapFrom(src => src.ExamDurationMinutes))
                .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceLevel))
                .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.NumberOfQuestions))
                .ForMember(dest => dest.ApplicationDeadline, opt => opt.MapFrom(src => src.ApplicationDeadline))
                .ForMember(dest => dest.ATSMinimumScore, opt => opt.MapFrom(src => src.ATSMinimumScore))
                .ForMember(dest => dest.AutoSend, opt => opt.MapFrom(src => src.AutoSend))
                .ForMember(dest => dest.HRId, opt => opt.MapFrom(src => src.HRId))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                    src.JobSkills != null && src.JobSkills.Any()
                        ? src.JobSkills.Select(js => new SkillResponseDto
                        {
                            Id = js.Skill.Id,
                            Title = js.Skill.Name,
                            Description = js.Skill.Description
                        }).ToList()
                        : new List<SkillResponseDto>()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.TotalApplications, opt => opt.MapFrom(src =>
                    src.Applications != null ? src.Applications.Count : 0))
                .ForMember(dest => dest.ExamsCompleted, opt => opt.MapFrom(src =>
                    src.Applications != null
                        ? src.Applications.Count(a => a.ExamStatus == enExamStatus.Completed)
                        : 0));

            //CreateMap<JobPost, JobPostResponseDto>()
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //.ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.JobStatus))
            //.ForMember(dest => dest.ExamDurationMinutes, opt => opt.MapFrom(src => src.ExamDurationMinutes))
            //.ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceLevel))
            //.ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType))
            //.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            //.ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
            //.ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.NumberOfQuestions))
            //.ForMember(dest => dest.ApplicationDeadline, opt => opt.MapFrom(src => src.ApplicationDeadline))
            //.ForMember(dest => dest.ATSMinimumScore, opt => opt.MapFrom(src => src.ATSMinimumScore))
            //.ForMember(dest => dest.AutoSend, opt => opt.MapFrom(src => src.AutoSend))
            //.ForMember(dest => dest.HRId, opt => opt.MapFrom(src => src.HRId))
            //.ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
            //    src.JobSkills != null && src.JobSkills.Any()
            //        ? src.JobSkills.Select(js => new SkillResponseDto
            //        {
            //            Id = js.Skill.Id,
            //            Title = js.Skill.Name,
            //            Description = js.Skill.Description
            //        }).ToList()
            //        : new List<SkillResponseDto>()))
            //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            //.ForMember(dest => dest.TotalApplications, opt => opt.Ignore())
            //.ForMember(dest => dest.ExamsCompleted, opt => opt.Ignore());

            // ==================== REVERSE MAPPINGS ====================
            // JobPostResponseDto -> JobPostRequestDto (If needed for re-submission)
            CreateMap<JobPostResponseDto, JobPostRequestDto>()
                .ForMember(dest => dest.HRId, opt => opt.MapFrom(src => src.HRId))
                .ForMember(dest => dest.SkillIds, opt => opt.MapFrom(src => src.Skills != null
                    ? src.Skills.Select(s => s.Id).ToList()
                    : new List<int>()));

            // JobPostResponseDto -> JobPostUpdateDto (Convert response to update format)
            CreateMap<JobPostResponseDto, JobPostUpdateDto>()
                .ForMember(dest => dest.SkillIds, opt => opt.MapFrom(src => src.Skills != null 
                    ? src.Skills.Select(s => s.Id).ToList() 
                    : new List<int>()));


            // ==================== JOB OPENING DTO MAPPINGS ====================
            // JobPost -> JobOpeningDTO (Public job listing view with enums as strings)
            CreateMap<JobPost, JobOpeningDTO>()
                .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.JobStatus))
                .ForMember(dest => dest.ExamDurationMinutes, opt => opt.MapFrom(src => src.ExamDurationMinutes))
                .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceLevel))
                .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.NumberOfQuestions))
                .ForMember(dest => dest.ApplicationDeadline, opt => opt.MapFrom(src => src.ApplicationDeadline))
                .ForMember(dest => dest.ATSMinimumScore, opt => opt.MapFrom(src => src.ATSMinimumScore));
        }
    }
}