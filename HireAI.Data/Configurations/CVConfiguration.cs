using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class CVConfiguration : IEntityTypeConfiguration<CV>
    {
        public void Configure(EntityTypeBuilder<CV> builder)
        {
            builder.HasKey(cv => cv.Id);

            builder.Property(cv => cv.Phone)
                .HasMaxLength(20);

            builder.Property(cv => cv.LinkedInPath)
                .HasMaxLength(500);

            builder.Property(cv => cv.GitHubPath)
                .HasMaxLength(500);

            builder.Property(cv => cv.Title)
                .HasMaxLength(100);

            builder.Property(cv => cv.Education)
                .HasMaxLength(1000);

            builder.Property(cv => cv.Experience)
                .HasMaxLength(2000);

            builder.Property(cv => cv.YearsOfExperience)
                .IsRequired(false);

            builder.Property(cv => cv.Certifications)
                .HasMaxLength(2000)
                .HasConversion(
                    v => v != null ? string.Join(";", v) : null,
                    v => v != null ? v.Split(";").ToList() : null);

            // Foreign Key
            builder.HasOne(cv => cv.Applicant)
                .WithOne(a => a.ApplicantCV)
                .HasForeignKey<CV>(cv => cv.ApplicantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Index
            builder.HasIndex(cv => cv.ApplicantId)
                .IsUnique();

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_CV_YearsOfExperience", "[YearsOfExperience] >= 0 OR [YearsOfExperience] IS NULL"));
        }
    }
}
