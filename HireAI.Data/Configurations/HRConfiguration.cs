using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class HRConfiguration : IEntityTypeConfiguration<HR>
    {
        public void Configure(EntityTypeBuilder<HR> builder)
        {
            builder.Property(hr => hr.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(hr => hr.AccountType)
                .IsRequired()
                .HasDefaultValue(0); // AccountType.Free

            builder.Property(hr => hr.PremiumExpiry)
                .IsRequired(false);

            // Navigation properties
            builder.HasMany(hr => hr.HRJobs)
                .WithOne(j => j.HR)
                .HasForeignKey(j => j.HRId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(hr => hr.Applications)
                .WithOne(a => a.HR)
                .HasForeignKey(a => a.HrId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(hr => hr.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(hr => hr.CompanyName);
            builder.HasIndex(hr => hr.AccountType);
        }
    }
}
