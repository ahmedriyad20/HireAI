using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();


            builder.Property(e => e.ExamName)
                .HasMaxLength(200);

            builder.Property(e => e.IsAi)
                .HasDefaultValue(true);


            //Type Conversion
            builder.Property(e => e.ExamType)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enExamType)Enum.Parse(typeof(enExamType), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enExamType.MockExam);

            builder.Property(e => e.ExamLevel)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enExamLevel)Enum.Parse(typeof(enExamLevel), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enExamLevel.Beginner);

            // Check constraints
            builder.ToTable(t => t.HasCheckConstraint("CK_Exam_NumberOfQuestions", "[NumberOfQuestions] > 0"));
            builder.ToTable(t => t.HasCheckConstraint("CK_Exam_Duration", "[DurationInMinutes] > 0"));
        }
    }
}
