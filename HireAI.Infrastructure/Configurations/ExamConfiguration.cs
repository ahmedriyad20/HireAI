using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.NumberOfQuestions)
                .IsRequired();

            builder.Property(e => e.DurationInMinutes)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.ExamName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.IsAi)
                .IsRequired()
                .HasDefaultValue(true);

            // Foreign Keys
           

            builder.HasOne(e => e.Application)
                .WithOne(a => a.Exam)
                .HasForeignKey<Exam>(e => e.ApplicationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Navigation property
       

            // Indexes
            builder.HasIndex(e => e.ApplicantId);
            builder.HasIndex(e => new { e.ApplicantId, e.CreatedAt });

            // Check constraints
            builder.ToTable(t => t.HasCheckConstraint("CK_Exam_NumberOfQuestions", "[NumberOfQuestions] > 0"));
            builder.ToTable(t => t.HasCheckConstraint("CK_Exam_Duration", "[DurationInMinutes] > 0"));
        }
    }
}
