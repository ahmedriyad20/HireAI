using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.Property(a => a.ResumeUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.CvId)
                .IsRequired();

            // Navigation properties
            builder.HasMany(a => a.ApplicantSkills)
                .WithOne(asn => asn.Applicant)
                .HasForeignKey(asn => asn.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Applications)
                .WithOne(app => app.Applicant)
                .HasForeignKey(app => app.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Exams)
                .WithOne(e => e.Applicant)
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.ExamAttempts)
                .WithMany()
                .UsingEntity("ApplicantExamAttempt");

            builder.HasOne(a => a.ApplicantCV)
                .WithOne(cv => cv.Applicant)
                .HasForeignKey<CV>(cv => cv.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.CvId);
        }
    }
}
