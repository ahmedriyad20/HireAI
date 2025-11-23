using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.Property(a => a.Id)
                              .ValueGeneratedOnAdd();
            builder.Property(a => a.Text)
                .HasColumnType("varchar(100)");

            builder.Property(a => a.IsCorrect)
                .HasDefaultValue(false);
            builder.Property(a => a.QuestionId)
           .IsRequired();

            // Correct FK: Answer -> Question (many answers per question)
            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.QuestionId);

            // Index for quick lookup of correct answers
            builder.HasIndex(a => new { a.QuestionId, a.IsCorrect });
        }
    }
}
