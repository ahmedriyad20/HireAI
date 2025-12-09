
using Amazon.S3;

using HireAI.Service.Services;

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
            services.AddScoped<IJobPostService, JobPostService>();
            services.AddScoped<IHrDashboardService, HRDashboardService>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<IReportPdfService, ReportPdfService>();
            services.AddScoped<IReportService,ReportService>();

            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IJobDetailsService, JobDetailsService>();


            return services;
        }
    }
}