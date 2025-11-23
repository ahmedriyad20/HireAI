using HireAI.Data.Helpers.Enums;
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

            // Foreign Keys
            builder.HasOne(ee => ee.ExamSummary)
                .WithOne(es => es.ExamEvaluation)
                .HasForeignKey<ExamEvaluation>(ee => ee.ExamSummaryId)
                .OnDelete(DeleteBehavior.Restrict);

            //Type Conversion
            builder.Property(j => j.Status)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enExamEvaluationStatus)Enum.Parse(typeof(enExamEvaluationStatus), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enAccountType.Free);

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
