using System;
using HireAI.Data.Helpers.Enums;
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
                .HasMaxLength(200);

            //Type Conversion (null-safe)
            builder.Property(u => u.Answer)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString() : null, // enum? -> string (null preserved)
                    v => string.IsNullOrEmpty(v) ? (enQuestionAnswers?)null : (enQuestionAnswers)Enum.Parse(typeof(enQuestionAnswers), v) // string -> enum?
                );

            // Foreign Keys
            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.ApplicantResponse)
                .WithOne(ar => ar.Question)
                .HasForeignKey<Question>(q => q.ApplicantResponseId)
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
