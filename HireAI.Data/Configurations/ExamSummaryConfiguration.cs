using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ExamSummaryConfiguration : IEntityTypeConfiguration<ExamSummary>
    {
        public void Configure(EntityTypeBuilder<ExamSummary> builder)
        {
            builder.HasKey(es => es.Id);

            builder.Property(es => es.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Foreign Keys
            builder.HasOne(es => es.Application)
                .WithOne(a => a.ExamSummary)
                .HasForeignKey<ExamSummary>(es => es.ApplicationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.Test)
                .WithMany()
                .HasForeignKey(es => es.ExamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.ExamEvaluation)
                .WithOne(ee => ee.ExamSummary)
                .HasForeignKey<ExamSummary>(es => es.ExamEvaluationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(es => es.ApplicationId)
                .IsUnique();

            builder.HasIndex(es => es.ExamId);
            builder.HasIndex(es => es.CreatedAt);
        }
    }
}
