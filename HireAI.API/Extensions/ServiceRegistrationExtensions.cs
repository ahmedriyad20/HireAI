using HireAI.Service.Implementation;
using HireAI.Service.Interfaces;

namespace HireAI.API.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicantJobPostService, ApplicantJobPostService>();
            services.AddScoped<IApplicantDashboardService, ApplicantDashboardService>();
            services.AddScoped<ApplicantDashboardService>();
            services.AddScoped<IApplicantApplicationService, ApplicantApplicationService>();
            services.AddScoped<ApplicantApplicationService>();
            services.AddScoped<IMockExamService, MockExamService>();
            services.AddScoped<MockExamService>();
            services.AddScoped<IHRService, HRService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IJobPostService, JopPostService>();
            services.AddScoped<IHrDashboardService, HRDashboardService>();

            return services;
        }
    }
}