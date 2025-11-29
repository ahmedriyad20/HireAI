using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
    {
        public void Configure(EntityTypeBuilder<JobPost> builder)
        {
          

            builder.Property(j => j.Id)
                .ValueGeneratedOnAdd();
           

            builder.Property(j => j.Title)
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .HasMaxLength(3000);

            builder.Property(j => j.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


            builder.Property(j => j.Location)
                .HasMaxLength(200);

            builder.Property(j => j.SalaryRange)
                .HasMaxLength(50);


            builder.Property(j => j.JobStatus)
             .HasConversion<int>()
                                                                                                                           // )
             .HasDefaultValue(enJobStatus.Active);

            builder.Property(u => u.ExperienceLevel)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString() : null, // enum? -> string (null preserved)
                    v => string.IsNullOrEmpty(v) ? (enExperienceLevel?)null : (enExperienceLevel)Enum.Parse(typeof(enExperienceLevel), v) // string -> enum?
                );
            


            builder.Property(u => u.EmploymentType)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString() : null, // enum? -> string (null preserved)
                    v => string.IsNullOrEmpty(v) ? (enEmploymentType?)null : (enEmploymentType)Enum.Parse(typeof(enEmploymentType), v) // string -> enum?
                );



            // Foreign Key
            builder.HasOne(j => j.HR)
                .WithMany(hr => hr.HRJobs)
                .HasForeignKey(j => j.HRId)
                .OnDelete(DeleteBehavior.Restrict);

            // Navigation properties
      
            builder.HasMany(j => j.Applications)
                .WithOne(a => a.AppliedJob)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(j => j.ExamEvaluations)
                .WithOne(ee => ee.JobPost)
                .HasForeignKey(ee => ee.JobId)
                .OnDelete(DeleteBehavior.Restrict);

    

            // Indexes
            builder.HasIndex(j => j.HRId);

            // Check constraints
            builder.ToTable(t => t.HasCheckConstraint("CK_JobPost_ExamDuration", "[ExamDurationMinutes] > 0 OR [ExamDurationMinutes] IS NULL"));
            builder.ToTable(t => t.HasCheckConstraint("CK_JobPost_Questions", "[NumberOfQuestions] > 0 OR [NumberOfQuestions] IS NULL"));
            builder.ToTable(t => t.HasCheckConstraint("CK_JobPost_ATSScore", "([ATSMinimumScore] >= 0 AND [ATSMinimumScore] <= 100) OR [ATSMinimumScore] IS NULL"));
        }
    }
}
