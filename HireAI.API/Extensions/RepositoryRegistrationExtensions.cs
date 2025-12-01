using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Infrastructure.Repositories;

namespace HireAI.API.Extensions
{
    public static class RepositoryRegistrationExtensions
    {
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IJobPostRepository, JobPostRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IExamSummaryRepository, ExamSummaryRepository>();
            services.AddScoped<IExamEvaluationRepository, ExamEvaluationRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IHRRepository, HRRepository>();
            services.AddScoped<IJobSkillRepository, JobSkillRepository>();

            return services;
        }
    }
}