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
            builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();


            builder.Property(q => q.QuestionText)
                .HasMaxLength(200);

            // Foreign Keys
            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            

            // Indexes
            builder.HasIndex(q => new { q.ExamId, q.QuestionNumber })
                .IsUnique();

            builder.HasIndex(q => q.ExamId);
        }
    }
}
