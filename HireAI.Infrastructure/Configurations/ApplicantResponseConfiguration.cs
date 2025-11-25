using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicantResponseConfiguration : IEntityTypeConfiguration<ApplicantResponse>
    {
        public void Configure(EntityTypeBuilder<ApplicantResponse> builder)
        {
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();



            // Foreign Keys
            builder.HasOne(ar => ar.Question)
                .WithMany()
                .HasForeignKey(ar => ar.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.ExamSummary)
                .WithMany()
                .HasForeignKey(ar => ar.ExamSummaryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Navigation property
            builder.HasOne(ar => ar.QuestionEvaluation)
                .WithOne(qe => qe.ApplicantResponse)
                .HasForeignKey<QuestionEvaluation>(qe => qe.ApplicantResponseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(ar => ar.ExamSummaryId);
            builder.HasIndex(ar => new { ar.ExamSummaryId, ar.QuestionId })
                .IsUnique();

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_ApplicantResponse_Answer", "[AnswerNumber] > 0"));
        }
    }
}
