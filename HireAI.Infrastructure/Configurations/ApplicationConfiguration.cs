using HireAI.Data.Helpers.Enums;
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


            builder.Property(a => a.DateApplied)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.CVFilePath)
                .HasMaxLength(500);

            // Foreign Keys
            builder.HasOne(a => a.HR)
                .WithMany(hr => hr.Applications)
                .HasForeignKey(a => a.HRId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(a => a.Exam)
                .WithOne(e => e.Application)
                .HasForeignKey<Application>(a => a.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ExamSummary)
                .WithOne(es => es.Application)
                .HasForeignKey<Application>(a => a.Id)
                .OnDelete(DeleteBehavior.SetNull);

            //Type Conversion
            builder.Property(a => a.ApplicationStatus)
            .HasConversion(
              v => v.ToString(),// Converts the enum to string when saving to the database                  
             v => (enApplicationStatus)Enum.Parse(typeof(enApplicationStatus), v)// Converts the string back to enum when reading from the database
              );

            // Indexes
            builder.HasIndex(a => a.HRId);
            builder.HasIndex(a => a.ApplicantId);
            builder.HasIndex(a => a.JobId);
            builder.HasIndex(a => new { a.ApplicantId, a.JobId })
                .IsUnique();
            builder.HasIndex(a => a.ApplicationStatus);

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_Application_Score", "([AtsScore] >= 0 AND [AtsScore] <= 100) OR [AtsScore] IS NULL"));
        }
    }
}
