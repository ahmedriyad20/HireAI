using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class QuestionEvaluationConfiguration : IEntityTypeConfiguration<QuestionEvaluation>
    {
        public void Configure(EntityTypeBuilder<QuestionEvaluation> builder)
        {
            builder.HasKey(qe => qe.Id);

            builder.Property(qe => qe.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(qe => qe.Feedback)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(qe => qe.EvaluatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Foreign Keys
            builder.HasOne(qe => qe.ApplicantResponse)
                .WithOne(ar => ar.QuestionEvaluation)
                .HasForeignKey<QuestionEvaluation>(qe => qe.ApplicantResponseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qe => qe.ExamEvaluation)
                .WithMany(ee => ee.QuestionEvaluations)
                .HasForeignKey(qe => qe.ExamEvaluationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Composite index for common queries
            builder.HasIndex(qe => new { qe.ExamEvaluationId, qe.ApplicantResponseId });
        }
    }
}
