using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ExamEvaluationConfiguration : IEntityTypeConfiguration<ExamEvaluation>
    {
        public void Configure(EntityTypeBuilder<ExamEvaluation> builder)
        {
            builder.HasKey(ee => ee.Id);

            builder.Property(ee => ee.TotalScore)
                .IsRequired();

            builder.Property(ee => ee.MaxTotal)
                .IsRequired();

            builder.Property(ee => ee.Passed)
                .IsRequired();

            builder.Property(ee => ee.EvaluatedAt)
                .IsRequired(false);

            builder.Property(ee => ee.Status)
                .IsRequired();

            // Foreign Keys
            builder.HasOne(ee => ee.ExamSummary)
                .WithOne(es => es.ExamEvaluation)
                .HasForeignKey<ExamEvaluation>(ee => ee.ExamSummaryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            // Navigation property
            builder.HasMany(ee => ee.QuestionEvaluations)
                .WithOne(qe => qe.ExamEvaluation)
                .HasForeignKey(qe => qe.ExamEvaluationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(ee => ee.ExamSummaryId)
                .IsUnique();

            builder.HasIndex(ee => ee.JobId);
            builder.HasIndex(ee => ee.Status);

            // Check constraints
            builder.ToTable(t => t.HasCheckConstraint("CK_ExamEvaluation_Scores", "[TotalScore] >= 0 AND [MaxTotal] > 0 AND [TotalScore] <= [MaxTotal]"));
        }
    }
}
