using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.Property(u => u.Bio)
                .HasMaxLength(500);

            builder.Property(u => u.Title)
                .HasMaxLength(100);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property(u => u.IsPremium)
                .HasDefaultValue(false);

            builder.Property(u => u.AccountPlan)
                .HasDefaultValue(0); // AccountType.Free

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.LastLogin)
                .IsRequired(false);

            // Unique constraint for Email
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Configure discriminator for TPH (Table Per Hierarchy)
            builder.HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<HR>("HR")
                .HasValue<Applicant>("Applicant");
        }
    }
}
