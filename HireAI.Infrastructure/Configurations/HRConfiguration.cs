using HireAI.Data.Helpers.Enums;
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
                .HasMaxLength(200);

            builder.Property(hr => hr.AccountType)
                 .HasDefaultValue(enAccountType.Free); //type convertions 

            builder.Property(hr => hr.PremiumExpiry)
                .IsRequired(false);


            builder.HasMany(hr => hr.Payments)
                .WithOne(p => p.HR)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(hr => hr.CompanyName);
            builder.HasIndex(hr => hr.AccountType);
        }
    }
}
