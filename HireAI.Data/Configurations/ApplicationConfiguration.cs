using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ApplicationStatus)
                .IsRequired();

            builder.Property(a => a.DateApplied)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.CVFilePath)
                .HasMaxLength(500);

            builder.Property(a => a.ScoreATS)
                .IsRequired(false);

            // Foreign Keys
            builder.HasOne(a => a.HR)
                .WithMany(hr => hr.Applications)
                .HasForeignKey(a => a.HrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Applicant)
                .WithMany(app => app.Applications)
                .HasForeignKey(a => a.ApplicantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.AppliedJob)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Exam)
                .WithOne(e => e.Application)
                .HasForeignKey<Application>(a => a.ExamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.ExamSummary)
                .WithOne(es => es.Application)
                .HasForeignKey<Application>(a => a.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => a.HrId);
            builder.HasIndex(a => a.ApplicantId);
            builder.HasIndex(a => a.JobId);
            builder.HasIndex(a => new { a.ApplicantId, a.JobId })
                .IsUnique();
            builder.HasIndex(a => a.ApplicationStatus);

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_Application_Score", "([ScoreATS] >= 0 AND [ScoreATS] <= 100) OR [ScoreATS] IS NULL"));
        }
    }
}
