using HireAI.Data;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Seeder
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HireAIDbContext>();

            // Apply migrations
            try
            {
                await context.Database.MigrateAsync();
            }
            catch
            {
                // Ignore migration errors
            }

            // Exit if JobOpenings already exist
            if (await context.JobOpenings.AnyAsync())
                return;

            // ======== Create HR ========


            var hr = new HR
            {
                Name = "HR One",
                Email = "hr1@example.com",
                Role = enRole.HR,
                CompanyName = "Acme Corp",
                AccountType = enAccountType.Free,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.HRs.Add(hr);
            await context.SaveChangesAsync();

            // ======== Create Job Openings ========
            var job1 = new JobOpening
            {
                Title = "Software Engineer",
                Description = "Build awesome software",
                HRId = hr.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                JobStatus = enJobStatus.Active
                ,
                CompanyName = "Acme Corp"
            };

            var job2 = new JobOpening
            {
                Title = "Senior Developer",
                Description = "Lead features",
                HRId = hr.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-60),
                JobStatus = enJobStatus.Active,
                CompanyName = "Acme Corp"

            };

            context.JobOpenings.AddRange(job1, job2);
            await context.SaveChangesAsync();

            // ======== Create Applicants ========
            var applicant1 = new Applicant
            {
                Name = "Alice Applicant",
                Email = "alice@example.com",
                ResumeUrl = "https://example.com/resume/alice.pdf",
                Role = enRole.Applicant,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                    Phone = "",           // <- add default value
                LastLogin = DateTime.UtcNow // <- add default value
            };

            var applicant2 = new Applicant
            {
                Name = "Bob Candidate",
                Email = "bob@example.com",
                ResumeUrl = "https://example.com/resume/bob.pdf",
                Role = enRole.Applicant,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-45) ,
                Phone = "",           // <- add default value
                LastLogin = DateTime.UtcNow // <- add default value
            };

            context.Applicants.AddRange(applicant1, applicant2);
            await context.SaveChangesAsync();

            // ======== Create Applications ========
            var application1 = new Application
            {
                ApplicantId = applicant1.Id,
                HRId = hr.Id,
                JobId = job1.Id,
                DateApplied = DateTime.UtcNow.AddDays(-12),
                AtsScore = 85f,
                ApplicationStatus = enApplicationStatus.UnderReview,
                ExamStatus = enExamStatus.completed
            };

            var application2 = new Application
            {
                ApplicantId = applicant2.Id,
                HRId = hr.Id,
                JobId = job1.Id,
                DateApplied = DateTime.UtcNow.AddDays(-40),
                AtsScore = 72f,
                ApplicationStatus = enApplicationStatus.UnderReview,
                ExamStatus = enExamStatus.completed
            };

            var application3 = new Application
            {
                ApplicantId = applicant1.Id,
                HRId = hr.Id,
                JobId = job2.Id,
                DateApplied = DateTime.UtcNow.AddDays(-10),
                AtsScore = 95f,
                ApplicationStatus = enApplicationStatus.UnderReview,
                ExamStatus = enExamStatus.completed


            };

            context.Applications.AddRange(application1, application2, application3);
            await context.SaveChangesAsync();

            // ======== Create Exams & Summaries ========
            var exam1 = new Exam
            {
                ApplicantId = applicant1.Id,
                ApplicationId = application1.Id,
                ExamName = "Coding Test",
                NumberOfQuestions = 5,
                DurationInMinutes = 30,
                CreatedAt = DateTime.UtcNow.AddDays(-11),
                IsAi = true
            };
            context.Exams.Add(exam1);
            await context.SaveChangesAsync();

            var summary1 = new ExamSummary
            {
                ApplicationId = application1.Id,
                ExamId = exam1.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                TotalScroe = 88f
            };
            context.ExamSummarys.Add(summary1);
            await context.SaveChangesAsync();

            // ======== Create Skills ========
            var skill1 = new Skill { Title = "C#", Description = "C# programming" };
            var skill2 = new Skill { Title = "SQL", Description = "Database" };
            context.Siklls.AddRange(skill1, skill2);
            await context.SaveChangesAsync();

            // ======== Applicant Skills ========
            var appSkill1 = new ApplicantSkill { ApplicantId = applicant1.Id, SkillId = skill1.Id, SkillRate = 90 };
            var appSkill2 = new ApplicantSkill { ApplicantId = applicant2.Id, SkillId = skill2.Id, SkillRate = 75 };
            context.ApplicantSkills.AddRange(appSkill1, appSkill2);
            await context.SaveChangesAsync();

            // ======== HR Payments ========
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = 99.99m,
                BillingPeriod = enBillingPeriod.Monthly,
                Currency = "USD",
                Status = enPaymentStatus.Completed,
                UpgradeTo = enAccountType.Pro,
                CreatedAt = DateTime.UtcNow,
                HrId = hr.Id,
                PaymentIntentId = Guid.NewGuid().ToString()
            };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();
        }
    }
}
