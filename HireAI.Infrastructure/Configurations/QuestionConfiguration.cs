using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.QuestionText)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(q => q.QuestionNumber)
                .IsRequired();

            // Foreign Keys
            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.ApplicantResponse)
                .WithOne(ar => ar.Question)
                .HasForeignKey<Question>(q => q.ApplicantResponseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Navigation property for answers
            builder.HasMany(q => q.Answers)
                .WithOne()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(q => new { q.ExamId, q.QuestionNumber })
                .IsUnique();

            builder.HasIndex(q => q.ExamId);
        }
    }
}
