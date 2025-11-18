using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicantResponseConfiguration : IEntityTypeConfiguration<ApplicantResponse>
    {
        public void Configure(EntityTypeBuilder<ApplicantResponse> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.Property(ar => ar.AnswerNumber)
                .IsRequired();

            // Foreign Keys
            builder.HasOne(ar => ar.Question)
                .WithMany()
                .HasForeignKey(ar => ar.QuestionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ar => ar.TestAttempt)
                .WithMany()
                .HasForeignKey(ar => ar.TestAttemptId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Navigation property
            builder.HasOne(ar => ar.QuestionEvaluation)
                .WithOne(qe => qe.ApplicantResponse)
                .HasForeignKey<QuestionEvaluation>(qe => qe.ApplicantResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(ar => ar.TestAttemptId);
            builder.HasIndex(ar => new { ar.TestAttemptId, ar.QuestionId })
                .IsUnique();

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_ApplicantResponse_Answer", "[AnswerNumber] > 0"));
        }
    }
}
