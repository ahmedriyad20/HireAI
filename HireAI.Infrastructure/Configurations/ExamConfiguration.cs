using HireAI.Data.Helpers.Enums;
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

            builder.Property(e => e.ExamName)
                .HasMaxLength(200);

            builder.Property(e => e.IsAi)
                .HasDefaultValue(true);

            // Foreign Keys
            builder.HasOne(e => e.Application)
                .WithOne(a => a.Exam)
                .HasForeignKey<Exam>(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            //Type Conversion
            builder.Property(e => e.ExamType)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enExamType)Enum.Parse(typeof(enExamType), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enExamType.MockExam);

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
