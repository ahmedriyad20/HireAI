using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;

using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Mappings;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Abstractions;
using HireAI.Service.Implementation;

using HireAI.Seeder;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddDbContext<HireAIDbContext>();

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

            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<IExamEvaluationRepository, ExamEvaluationRepository>();
            builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
            builder.Services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
            builder.Services.AddScoped<IApplicantDashboardService, ApplicantDashboardService>();
            builder.Services.AddScoped<ApplicantDashboardService>();
            #endregion

            #region Add AutoMapper service
            builder.Services.AddAutoMapper(cfg => { }, typeof(ApplicationProfile).Assembly);
            #endregion


            var app = builder.Build();

            // Apply migrations & seed database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Apply migrations
                    var context = services.GetRequiredService<HireAIDbContext>();
                    await context.Database.MigrateAsync();

                    // Seed data
                    await DbSeeder.SeedAsync(services);
                    Console.WriteLine("Database seeded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
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
