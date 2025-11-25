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

            // If already seeded, exit
            if (await context.JobOpenings.AnyAsync() || await context.Applicants.AnyAsync())
                return;

            var rnd = new Random();

            // ======== Create one HR ========
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

            // ======== Create one JobOpening for that HR ========
            var job = new JobOpening
            {
                Title = "Software Engineer",
                CompanyName = hr.CompanyName,
                Description = "Build awesome software",
                HRId = hr.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                JobStatus = enJobStatus.Active,
                ExamDurationMinutes = 60,
                NumberOfQuestions = 10,
                ApplicationDeadline = DateTime.UtcNow.AddMonths(1),
                ATSMinimumScore = 60
            };
            context.JobOpenings.Add(job);
            await context.SaveChangesAsync();

            // ======== Create some Skills and link to Job ========
            var skills = new List<Skill>
            {
                new Skill { Title = "C#", Description = "C# programming" },
                new Skill { Title = "SQL", Description = "Database design and queries" },
                new Skill { Title = "ASP.NET", Description = "Web development" }
            };
            context.Siklls.AddRange(skills);
            await context.SaveChangesAsync();

            var jobSkills = skills.Select(s => new JobSkill { JobId = job.Id, SkillId = s.Id }).ToList();
            context.JobSkills.AddRange(jobSkills);
            await context.SaveChangesAsync();

            // ======== Create many Applicants (1000) ========
            const int applicantCount = 1000;
            var applicants = new List<Applicant>(applicantCount);
            for (int i = 1; i <= applicantCount; i++)
            {
                applicants.Add(new Applicant
                {
                    Name = $"Applicant {i}",
                    Email = $"applicant{i}@example.com",
                    ResumeUrl = $"https://example.com/resume/applicant{i}.pdf",
                    Role = enRole.Applicant,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 180)),
                    Phone = $"555-000-{i:D4}",
                    LastLogin = DateTime.UtcNow.AddDays(-rnd.Next(0, 30)),
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
                    Title = "CV - " + a.Name,
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
                    DateApplied = DateTime.UtcNow.AddDays(-rnd.Next(0, 180)), // spread dates
                    AtsScore = (float?)rnd.Next(40, 101),
                    ApplicationStatus = (enApplicationStatus)(rnd.Next(0, Enum.GetValues(typeof(enApplicationStatus)).Length)),
                    ExamStatus = rnd.NextDouble() > 0.3 ? enExamStatus.completed : enExamStatus.notTaken // ~70% completed
                };
                // set CV file path from applicant resume
                ap.CVFilePath = applicants[i].ResumeUrl;
                applications.Add(ap);
            }
            context.Applications.AddRange(applications);
            await context.SaveChangesAsync();

            // ======== Create Exams for completed applications and Questions/Answers ========
            var exams = new List<Exam>();
            var summaries = new List<ExamSummary>();
            var questions = new List<Question>();
            var answers = new List<Answer>();
            var applicantResponses = new List<ApplicantResponse>();

            int examCounter = 0;
            foreach (var appItem in applications.Where(a => a.ExamStatus == enExamStatus.completed))
            {
                examCounter++;
                var exam = new Exam
                {
                    ApplicantId = appItem.ApplicantId,
                    ApplicationId = appItem.Id,
                    ExamName = $"Coding Test #{examCounter}",
                    NumberOfQuestions = job.NumberOfQuestions ?? 5,
                    DurationInMinutes = job.ExamDurationMinutes ?? 30,
                    CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 90)),
                    IsAi = true
                };
                exams.Add(exam);
            }
            context.Exams.AddRange(exams);
            await context.SaveChangesAsync();

            // Link exam ids back to applications (one-to-one) and create summaries and questions
            int qCounter = 0;
            foreach (var ex in exams)
            {
                // update corresponding application
                var relatedApp = applications.First(a => a.Id == ex.ApplicationId);
                relatedApp.ExamId = ex.Id;
                context.Applications.Update(relatedApp);

                // Create summary
                var summary = new ExamSummary
                {
                    ApplicationId = relatedApp.Id,
                    ExamId = ex.Id,
                    CreatedAt = ex.CreatedAt.AddDays(1),
                    TotalScroe = (float)rnd.Next(50, 101)
                };
                summaries.Add(summary);

                // Create questions for this exam (vary count)
                int qCount = Math.Max(3, ex.NumberOfQuestions);
                for (int q = 1; q <= qCount; q++)
                {
                    qCounter++;
                    var question = new Question
                    {
                        ExamId = ex.Id,
                        QuestionText = $"Question {q} for Exam {ex.Id}",
                        QuestionNumber = q,
                        Answer = enQuestionAnswers.A
                    };
                    questions.Add(question);
                }
            }
            context.ExamSummarys.AddRange(summaries);
            context.Questions.AddRange(questions);
            await context.SaveChangesAsync();

            // Create answers for questions (4 answers each)
            foreach (var q in questions)
            {
                for (int a = 1; a <= 4; a++)
                {
                    answers.Add(new Answer
                    {
                        QuestionId = q.Id,
                        Text = $"Option {a} for Q{q.Id}",
                        IsCorrect = (a == 1) // first option correct for seeded data
                    });
                }
            }
            context.Answers.AddRange(answers);
            await context.SaveChangesAsync();

            // Create ApplicantResponse rows for some summaries/questions to vary data
            foreach (var s in summaries)
            {
                // take first few questions of the exam
                var examQs = questions.Where(q => q.ExamId == s.ExamId).Take(3).ToList();
                int answerNum = 1;
                foreach (var q in examQs)
                {
                    applicantResponses.Add(new ApplicantResponse
                    {
                        ExamSummaryId = s.Id,
                        QuestionId = q.Id,
                        AnswerNumber = answerNum++
                    });
                }
            }
            context.ApplicantResponses.AddRange(applicantResponses);
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
                        TotalScore = s.TotalScroe,
                        MaxTotal = 100f,
                        IsPassed = s.TotalScroe >= 60f,
                        EvaluatedAt = s.CreatedAt.AddDays(1),
                        Status = enExamEvaluationStatus.Completed
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

            // ======== Create QuestionEvaluations for some ApplicantResponses ========
            var questionEvaluations = new List<QuestionEvaluation>();
            foreach (var ar in applicantResponses)
            {
                // try find matching examEvaluation for this response's examSummary
                var ev = examEvaluations.FirstOrDefault(x => x.ExamSummaryId == ar.ExamSummaryId);
                if (ev == null) continue; // skip if no evaluation created

                // create evaluation for some responses
                if (rnd.NextDouble() > 0.4)
                {
                    var qEval = new QuestionEvaluation
                    {
                        ApplicantResponseId = ar.Id,
                        ExamEvaluationId = ev.Id,
                        Feedback = "Reviewed: auto-seed feedback",
                        IsCorrect = rnd.NextDouble() > 0.3,
                        EvaluatedAt = DateTime.UtcNow
                    };
                    questionEvaluations.Add(qEval);
                }
            }
            if (questionEvaluations.Any())
            {
                context.QuestionEvaluations.AddRange(questionEvaluations);
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

            // ======== HR Payments (a few entries) ========
            var payments = new List<Payment>
            {
                new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = 49.99m,
                    BillingPeriod = enBillingPeriod.Monthly,
                    Currency = "USD",
                    Status = enPaymentStatus.Completed,
                    UpgradeTo = enAccountType.Free,
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                    HrId = hr.Id,
                    PaymentIntentId = Guid.NewGuid().ToString()
                },
                new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = 99.99m,
                    BillingPeriod = enBillingPeriod.Monthly,
                    Currency = "USD",
                    Status = enPaymentStatus.Completed,
                    UpgradeTo = enAccountType.Pro,
                    CreatedAt = DateTime.UtcNow.AddMonths(-1),
                    HrId = hr.Id,
                    PaymentIntentId = Guid.NewGuid().ToString()
                }
            };
            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();
        }
    }
}
