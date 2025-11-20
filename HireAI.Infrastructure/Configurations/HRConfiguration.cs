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


            //Type Conversion
            builder.Property(j => j.AccountType)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enAccountType)Enum.Parse(typeof(enAccountType), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enAccountType.Free);

            builder.HasMany(hr => hr.Payments)
                .WithOne(p => p.HR)

                .HasForeignKey(p => p.
                Id)

                .HasForeignKey(p => p.HrId)

                .OnDelete(DeleteBehavior.Restrict);
           
            // Indexes
            builder.HasIndex(hr => hr.CompanyName);
            builder.HasIndex(hr => hr.AccountType);
        }
    }
}
