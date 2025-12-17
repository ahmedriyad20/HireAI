using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class 
        
        Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(255);

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.Property(u => u.Bio)
                .HasMaxLength(500);

            builder.Property(u => u.Title)
                .HasMaxLength(100);

            builder.Property(u => u.IsPremium)
                .HasDefaultValue(false);

            //Type Conversion
            builder.Property(u => u.Role)
                  .HasConversion(
                v => v.ToString(),// Converts the enum to string when saving to the database                  
               v => (enRole)Enum.Parse(typeof(enRole), v)// Converts the string back to enum when reading from the database
                );


            // Unique constraint for Email
            builder.HasIndex(u => u.Email)
                .IsUnique();


        }
    }
}
