using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class JobOpeningConfiguration : IEntityTypeConfiguration<JobOpening>
    {
        public void Configure(EntityTypeBuilder<JobOpening> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .HasMaxLength(3000);

            builder.Property(j => j.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(j => j.Status)
                .IsRequired()
                .HasDefaultValue(0); // JobStatus.Open

            builder.Property(j => j.ExamDurationMinutes)
                .IsRequired(false);

            builder.Property(j => j.ExperienceLevel)
                .IsRequired(false);

            builder.Property(j => j.EmploymentType)
                .IsRequired(false);

            builder.Property(j => j.Location)
                .HasMaxLength(200);

            builder.Property(j => j.SalaryRange)
                .HasMaxLength(50);

            builder.Property(j => j.NumberOfQuestions)
                .IsRequired(false);

            builder.Property(j => j.ApplicationDeadline)
                .IsRequired(false);

            builder.Property(j => j.ATSMinimumScore)
                .IsRequired(false);

            builder.Property(j => j.AutoSend)
                .HasDefaultValue(false);

            // Foreign Key
            builder.HasOne(j => j.HR)
                .WithMany(hr => hr.HRJobs)
                .HasForeignKey(j => j.HRId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Navigation properties
            builder.HasMany(j => j.JobSkills)
                .WithOne(js => js.Job)
                .HasForeignKey(js => js.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Applications)
                .WithOne(a => a.AppliedJob)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.ExamEvaluations)
                .WithOne(ee => ee.JobOpening)
                .HasForeignKey(ee => ee.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Applicants)
                .WithMany()
                .UsingEntity("JobApplicant");

            // Indexes
            builder.HasIndex(j => j.HRId);
            builder.HasIndex(j => j.Status);
            builder.HasIndex(j => new { j.HRId, j.Status });

            // Check constraints
            builder.ToTable(t => t.HasCheckConstraint("CK_JobOpening_ExamDuration", "[ExamDurationMinutes] > 0 OR [ExamDurationMinutes] IS NULL"));
            builder.ToTable(t => t.HasCheckConstraint("CK_JobOpening_Questions", "[NumberOfQuestions] > 0 OR [NumberOfQuestions] IS NULL"));
            builder.ToTable(t => t.HasCheckConstraint("CK_JobOpening_ATSScore", "([ATSMinimumScore] >= 0 AND [ATSMinimumScore] <= 100) OR [ATSMinimumScore] IS NULL"));
        }
    }
}
