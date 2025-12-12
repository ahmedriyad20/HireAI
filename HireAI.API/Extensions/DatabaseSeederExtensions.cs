using HireAI.Infrastructure.Context;
using HireAI.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HireAI.Data.Models.Identity;
using HireAI.Data.Models;
using HireAI.Data.Helpers.Enums;

namespace HireAI.API.Extensions
{
    public static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var context = services.GetRequiredService<HireAIDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

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

                    // Seed Identity Roles
                    await SeedRolesAsync(roleManager, logger);

                    // Seed Admin Accounts
                    await SeedAdminAccountsAsync(context, userManager, logger);

                    // Pass the ROOT service provider (app.Services), not the scoped one
                    await DbSeeder.SeedAsync(app.Services);
                    logger.LogInformation("Database seeding finished.");
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roles = { "HR", "Applicant" };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Role '{role}' created.");
                }
            }
        }

        private static async Task SeedAdminAccountsAsync(
            HireAIDbContext context, 
            UserManager<ApplicationUser> userManager, 
            ILogger logger)
        {
            // Seed Admin HR Account
            var hrAdminEmail = "admin@hr.com";
            var hrAdminUser = await userManager.FindByEmailAsync(hrAdminEmail);
            
            if (hrAdminUser == null)
            {
                // Create HR entity first
                var hrEntity = new HR
                {
                    FullName = "Admin HR",
                    Email = hrAdminEmail,
                    Address = "Admin Office",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Role = enRole.HR,
                    CompanyName = "HireAI Admin",
                    CompanyDescription = "System Administrator",
                    AccountType = enAccountType.Premium,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    Phone = "000-000-0000"
                };
                
                context.HRs.Add(hrEntity);
                await context.SaveChangesAsync();

                // Create Identity user
                hrAdminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = hrAdminEmail,
                    EmailConfirmed = true,
                    HRId = hrEntity.Id
                };

                var result = await userManager.CreateAsync(hrAdminUser, "admin");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(hrAdminUser, "HR");
                    logger.LogInformation("Admin HR account created successfully (Username: admin, Email: admin@hr.com).");
                }
                else
                {
                    logger.LogError($"Failed to create Admin HR: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Seed Admin Applicant Account
            var applicantAdminEmail = "admin@applicant.com";
            var applicantAdminUser = await userManager.FindByEmailAsync(applicantAdminEmail);
            
            if (applicantAdminUser == null)
            {
                // Create Applicant entity first
                var applicantEntity = new Applicant
                {
                    FullName = "Admin Applicant",
                    Email = applicantAdminEmail,
                    Address = "Admin Location",
                    DateOfBirth = new DateOnly(1995, 1, 1),
                    Role = enRole.Applicant,
                    ResumeUrl = "https://example.com/admin-resume.pdf",
                    SkillLevel = enSkillLevel.Expert,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    Phone = "000-000-0001"
                };
                
                context.Applicants.Add(applicantEntity);
                await context.SaveChangesAsync();

                // Create Identity user - Note: Using same username will work because Identity uses email as unique identifier
                applicantAdminUser = new ApplicationUser
                {
                    UserName = "applicant_admin",
                    Email = applicantAdminEmail,
                    EmailConfirmed = true,
                    ApplicantId = applicantEntity.Id
                };

                var result = await userManager.CreateAsync(applicantAdminUser, "admin");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicantAdminUser, "Applicant");
                    logger.LogInformation("Admin Applicant account created successfully (Username: applicant_admin, Email: admin@applicant.com).");
                }
                else
                {
                    logger.LogError($"Failed to create Admin Applicant: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}