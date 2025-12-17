using HireAI.Data;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireAI.Seeder
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, bool forceReseed = false)
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

            // Seed mock exams if they don't exist (always check this)
            await SeedMockExamsIfNeededAsync(context);

            // If already seeded and not forcing reseed, exit
            // Check for specific seeded HR to avoid conflict with admin accounts
            if (!forceReseed && await context.HRs.AnyAsync(h => h.Email == "hr1@example.com"))
                return;

            // If forcing reseed, clear existing data first
            if (forceReseed)
            {
                // Clear in reverse dependency order
                context.QuestionEvaluations.RemoveRange(context.QuestionEvaluations);
                context.ApplicantResponses.RemoveRange(context.ApplicantResponses);
                context.ExamEvaluations.RemoveRange(context.ExamEvaluations);
                context.ExamSummarys.RemoveRange(context.ExamSummarys);
                context.Answers.RemoveRange(context.Answers);
                context.Questions.RemoveRange(context.Questions);
                context.Exams.RemoveRange(context.Exams);
                context.Applications.RemoveRange(context.Applications);
                context.ApplicantSkills.RemoveRange(context.ApplicantSkills);
                context.CVs.RemoveRange(context.CVs);
                context.Applicants.RemoveRange(context.Applicants);
                context.JobSkills.RemoveRange(context.JobSkills);
                context.Skills.RemoveRange(context.Skills);
                context.JobPosts.RemoveRange(context.JobPosts);
                context.HRs.RemoveRange(context.HRs);
                await context.SaveChangesAsync();
            }

            var rnd = new Random();

            // ======== Create one HR ========
            var hr = new HR
            {
                FullName = "HR One",
                Email = "hr1@example.com",
                Address = "caior ",
                Role = enRole.HR,
                CompanyName = "Acme Corp",
                AccountType = enAccountType.Free,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            context.HRs.Add(hr);
            await context.SaveChangesAsync();

            // ======== Create one JobOpening for that HR ========
            var job = new JobPost
            {
                Title = "Software Engineer",
                CompanyName = hr.CompanyName,
                Description = "Build awesome software",
                HRId = hr.Id,
                CreatedAt = DateTime.Now.AddDays(-30),
                JobStatus = enJobStatus.Active,
                ExamDurationMinutes = 60,
                NumberOfQuestions = 10,
                ApplicationDeadline = DateTime.Now.AddMonths(1),
                ATSMinimumScore = 60
            };
            context.JobPosts.Add(job);
            await context.SaveChangesAsync();

            // ======== Create some Skills and link to Job ========
            var skills = new List<Skill>
            {
                new Skill { Name = "C#", Description = "C# programming" },
                new Skill { Name = "SQL", Description = "Database design and queries" },
                new Skill { Name = "ASP.NET", Description = "Web development" }
            };
            context.Skills.AddRange(skills);
            await context.SaveChangesAsync();

            var jobSkills = skills.Select(s => new JobSkill { JobId = job.Id, SkillId = s.Id }).ToList();
            context.JobSkills.AddRange(jobSkills);
            await context.SaveChangesAsync();

            // ======== Create many Applicants (100) ========
            const int applicantCount = 100;
            var applicants = new List<Applicant>(applicantCount);
            for (int i = 1; i <= applicantCount; i++)
            {
                applicants.Add(new Applicant
                {
                    FullName = $"Applicant {i}",
                    Email = $"applicant{i}@example.com",
                    ResumeUrl = $"https://example.com/resume/applicant{i}.pdf",
                    Role = enRole.Applicant,
                    Address = "fayouj",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 180)),
                    Phone = $"555-000-{i:D4}",
                    LastLogin = DateTime.Now.AddDays(-rnd.Next(0, 30)),
                    SkillLevel = (enSkillLevel)(rnd.Next(0, 3))
                });
            }
            context.Applicants.AddRange(applicants);
            await context.SaveChangesAsync();

            // ======== Create CVs for each Applicant ========
            var cvs = new List<CV>(applicantCount);
            foreach (var a in applicants)
            {
                var c = new CV
                {
                    ApplicantId = a.Id,
                    Title = "CV - " + a.FullName,
                    Phone = a.Phone,
                    Experience = $"{rnd.Next(1, 8)} years experience",
                    Education = "BSc in Computer Science",
                    YearsOfExperience = rnd.Next(0, 10),
                    Certifications = new List<string> { "Cert A", "Cert B" }
                };
                cvs.Add(c);
            }
            context.CVs.AddRange(cvs);
            await context.SaveChangesAsync();

            // Update applicants CVId
            foreach (var a in applicants)
            {
                var cv = cvs.First(c => c.ApplicantId == a.Id);
                a.CVId = cv.Id;
            }
            context.Applicants.UpdateRange(applicants);
            await context.SaveChangesAsync();

            // ======== Create many Applications (one per applicant) for job from different applicants ========
            var applications = new List<Application>(applicantCount);
            for (int i = 0; i < applicants.Count; i++)
            {
                var ap = new Application
                {
                    ApplicantId = applicants[i].Id,
                    HRId = hr.Id,          // use FK always 1 (the HR created above)
                    JobId = job.Id,
                    DateApplied = DateTime.Now.AddDays(-rnd.Next(0, 180)), // spread dates
                    AtsScore = (float?)rnd.Next(40, 101),
                    ApplicationStatus = (enApplicationStatus)(rnd.Next(0, Enum.GetValues(typeof(enApplicationStatus)).Length)),
                    ExamStatus = rnd.NextDouble() > 0.3 ? enExamStatus.Completed : enExamStatus.NotTaken // ~70% completed
                };
                // set CV file path from applicant resume
                ap.CVFilePath = applicants[i].ResumeUrl;
                applications.Add(ap);
            }
            context.Applications.AddRange(applications);
            await context.SaveChangesAsync();

            // ======== Create Exams for completed applications ========
            var exams = new List<Exam>();
            var summaries = new List<ExamSummary>();

            foreach (var appItem in applications.Where(a => a.ExamStatus == enExamStatus.Completed))
            {
                var exam = new Exam
                {
                    ExamName = "Job Application Exam",
                    ExamDescription = "Technical assessment for job application",
                    ExamLevel = enExamLevel.Intermediate,
                    ExamType = enExamType.HrExam,
                    NumberOfQuestions = job.NumberOfQuestions ?? 5,
                    DurationInMinutes = job.ExamDurationMinutes ?? 30,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 90)),
                    IsAi = true
                };
                exams.Add(exam);
            }
            context.Exams.AddRange(exams);
            await context.SaveChangesAsync();

            // Link exam ids back to applications (one-to-one) and create summaries
            var completedApps = applications.Where(a => a.ExamStatus == enExamStatus.Completed).ToList();
            for (int i = 0; i < exams.Count; i++)
            {
                var ex = exams[i];
                var relatedApp = completedApps[i];
                relatedApp.ExamId = ex.Id;

                // Create summary
                var summary = new ExamSummary
                {
                    ApplicationId = relatedApp.Id,
                    ExamId = ex.Id,
                    AppliedAt = ex.CreatedAt.AddDays(1),
                    ApplicantExamScore = (float)rnd.Next(50, 101)
                };
                summaries.Add(summary);
            }
            context.Applications.UpdateRange(completedApps);
            context.ExamSummarys.AddRange(summaries);
            await context.SaveChangesAsync();

            // ======== Create ExamEvaluations (one per some summaries) ========
            var examEvaluations = new List<ExamEvaluation>();
            foreach (var s in summaries)
            {
                // create an evaluation for roughly half of summaries
                if (rnd.NextDouble() > 0.5)
                {
                    var ev = new ExamEvaluation
                    {
                        ExamSummaryId = s.Id,
                        JobId = job.Id,
                        ApplicantExamScore = s.ApplicantExamScore,
                        ExamTotalScore = 100f,
                        EvaluatedAt = s.AppliedAt.AddDays(1),
                        Status = enExamEvaluationStatus.Passed
                    };
                    examEvaluations.Add(ev);
                }
            }
            if (examEvaluations.Any())
            {
                context.ExamEvaluations.AddRange(examEvaluations);
                await context.SaveChangesAsync();
            }

            // Link some ExamEvaluations back to ExamSummary (set ExamSummary.ExamEvaluationId)
            foreach (var ev in examEvaluations)
            {
                var summary = summaries.FirstOrDefault(s => s.Id == ev.ExamSummaryId);

                if (summary != null)
                {
                    summary.ExamEvaluationId = ev.Id;
                }
            }
            if (summaries.Any(s => s.ExamEvaluationId.HasValue))
            {
                context.ExamSummarys.UpdateRange(summaries.Where(s => s.ExamEvaluationId.HasValue));
                await context.SaveChangesAsync();
            }

            // ======== Applicant Skills ========
            var applicantSkills = new List<ApplicantSkill>();
            foreach (var a in applicants)
            {
                var skill = skills[rnd.Next(skills.Count)];
                applicantSkills.Add(new ApplicantSkill
                {
                    ApplicantId = a.Id,     
                    SkillId = skill.Id,
                    SkillRate = rnd.Next(50, 101)
                });
            }
            context.ApplicantSkills.AddRange(applicantSkills);
            await context.SaveChangesAsync();


        }

        /// <summary>
        /// Seeds mock exams if they don't already exist in the database
        /// </summary>
        private static async Task SeedMockExamsIfNeededAsync(HireAIDbContext context)
        {
            // Check if mock exams already exist
            if (await context.Exams.AnyAsync(e => e.ExamName == "C# Fundamentals" && e.ExamType == enExamType.MockExam))
                return;

            var rnd = new Random();

            var mockExams = new List<Exam>
            {
                new Exam
                {
                    ExamName = "C# Fundamentals",
                    ExamDescription = "Test your knowledge of C# programming fundamentals including variables, data types, control structures, loops, methods, classes, inheritance, polymorphism, interfaces, and exception handling.",
                    ExamLevel = enExamLevel.Beginner,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 30,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "ASP.NET Core Web API",
                    ExamDescription = "Evaluate your understanding of ASP.NET Core Web API development including RESTful services, controllers, routing, middleware, dependency injection, authentication, authorization, and Entity Framework Core.",
                    ExamLevel = enExamLevel.Intermediate,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 45,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "SQL Database Design",
                    ExamDescription = "Test your SQL skills including database design, normalization, joins, subqueries, stored procedures, indexes, transactions, and performance optimization techniques.",
                    ExamLevel = enExamLevel.Intermediate,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 40,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "Data Structures & Algorithms",
                    ExamDescription = "Assess your knowledge of fundamental data structures like arrays, linked lists, stacks, queues, trees, graphs, hash tables, and algorithms including sorting, searching, recursion, and dynamic programming.",
                    ExamLevel = enExamLevel.Advanced,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 60,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "JavaScript Essentials",
                    ExamDescription = "Test your JavaScript knowledge including ES6+ features, closures, promises, async/await, DOM manipulation, event handling, and modern JavaScript best practices.",
                    ExamLevel = enExamLevel.Beginner,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 30,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "React Development",
                    ExamDescription = "Evaluate your React.js skills including components, props, state, hooks, context API, Redux, routing, lifecycle methods, and React best practices for building modern web applications.",
                    ExamLevel = enExamLevel.Intermediate,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 45,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "Angular Framework",
                    ExamDescription = "Test your Angular knowledge including components, services, dependency injection, routing, forms, HTTP client, RxJS observables, and Angular CLI best practices.",
                    ExamLevel = enExamLevel.Intermediate,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 45,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "Design Patterns",
                    ExamDescription = "Assess your understanding of software design patterns including creational patterns (Singleton, Factory, Builder), structural patterns (Adapter, Decorator, Facade), and behavioral patterns (Observer, Strategy, Command).",
                    ExamLevel = enExamLevel.Advanced,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 50,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "Git Version Control",
                    ExamDescription = "Test your Git skills including branching, merging, rebasing, cherry-picking, resolving conflicts, Git flow workflow, and collaboration best practices for version control.",
                    ExamLevel = enExamLevel.Beginner,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 25,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                },
                new Exam
                {
                    ExamName = "Cloud Computing with Azure",
                    ExamDescription = "Evaluate your Microsoft Azure knowledge including Azure App Services, Azure Functions, Azure SQL, Blob Storage, Azure DevOps, virtual machines, and cloud architecture principles.",
                    ExamLevel = enExamLevel.Advanced,
                    ExamType = enExamType.MockExam,
                    NumberOfQuestions = 10,
                    DurationInMinutes = 60,
                    CreatedAt = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                    IsAi = true
                }
            };

            context.Exams.AddRange(mockExams);
            await context.SaveChangesAsync();
        }
    }
}
