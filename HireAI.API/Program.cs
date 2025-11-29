using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;

using HireAI.Infrastructure.GenaricBasies;

using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Mappings;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Implementation;

using HireAI.Seeder;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HireAI.Service.Interfaces;



namespace HireAI.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add Swagger generation service
            builder.Services.AddSwaggerGen();
            //builder.Services.AddDbContext<HireAIDbContext>();

            #region Register DbContext and Services in the DI Container
            builder.Services.AddDbContext<HireAIDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HireAiDB"));
            });

            //REGISTER ApplicationUser and IdentityRole with the DI Container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                //Lockout Settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<HireAIDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
            builder.Services.AddScoped<IApplicantJobPostService,ApplicantJobPostService>();

            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<IExamEvaluationRepository, ExamEvaluationRepository>();
            builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
            builder.Services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
            builder.Services.AddScoped<IApplicantDashboardService, ApplicantDashboardService>();
            builder.Services.AddScoped<ApplicantDashboardService>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

            builder.Services.AddScoped<IHRService, HRService>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

            builder.Services.AddScoped<IHRRepository, HRRepository>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IHRDashboardService, HRDashboardService>();



            #endregion

            #region Add AutoMapper service
            builder.Services.AddAutoMapper(cfg => { },  typeof(ApplicationProfile).Assembly);
            #endregion


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var context = services.GetRequiredService<HireAIDbContext>();

                    // Check if there are any pending migrations first (safe & fast)
                    var pending = await context.Database.GetPendingMigrationsAsync();
                    if (pending.Any())
                    {
                        logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Database migrated successfully.");

                    }
                    else
                    {
                        logger.LogInformation("No pending migrations.");
                    }

                    // Optional: call your seed method here
                    await DbSeeder.SeedAsync(services);
                    logger.LogInformation("Database seeding finished.");

                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    // In production you might rethrow or stop startup depending on policy.
                    // throw;
                }
            }

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
