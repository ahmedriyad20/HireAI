using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ExamSummaryConfiguration : IEntityTypeConfiguration<ExamSummary>
    {
        public void Configure(EntityTypeBuilder<ExamSummary> builder)
        {
            builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();


            builder.Property(es => es.AppliedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Foreign Keys
            builder.HasOne(es => es.Application)
                .WithOne(a => a.ExamSummary)
                .HasForeignKey<ExamSummary>(es => es.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.Exam)
                .WithMany()
                .HasForeignKey(es => es.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.ExamEvaluation)
                .WithOne(ee => ee.ExamSummary)
                .HasForeignKey<ExamSummary>(es => es.ExamEvaluationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(es => es.ApplicationId)
                .IsUnique();

            builder.HasIndex(es => es.ExamId);
            builder.HasIndex(es => es.AppliedAt);
        }
    }
}
