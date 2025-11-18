using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(a => a.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.QuestionId)
                .IsRequired();

            // Foreign Key - configured in QuestionConfiguration
            // But we can also add it here for completeness
            builder.HasIndex(a => a.QuestionId);

            // Index for quick lookup of correct answers
            builder.HasIndex(a => new { a.QuestionId, a.IsCorrect });
        }
    }
}
