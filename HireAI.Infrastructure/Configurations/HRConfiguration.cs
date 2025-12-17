using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HireAI.Data.Configurations
{
    public class HRConfiguration : IEntityTypeConfiguration<HR>
    {
        public void Configure(EntityTypeBuilder<HR> builder)

        {
            builder.Property(e => e.Id)
              .ValueGeneratedOnAdd(); 
            builder.Property(hr => hr.CompanyName)
                .HasMaxLength(200);

        

            //Type Conversion
            builder.Property(j => j.AccountType)
             .HasConversion(
               v => v.ToString(),// Converts the enum to string when saving to the database                  
              v => (enAccountType)Enum.Parse(typeof(enAccountType), v)// Converts the string back to enum when reading from the database
               )
             .HasDefaultValue(enAccountType.Free);

//            builder.Entity<JobPost>()
//.HasOne(jp => jp.HR)
//.WithMany(hr => hr.HRJobs)
//.HasForeignKey(jp => jp.HRId)
//.OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<Application>()
//    .HasOne(a => a.HR)
//    .WithMany(hr => hr.Applications)
//    .HasForeignKey(a => a.HRId)
//    .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(hr => hr.CompanyName);
            builder.HasIndex(hr => hr.AccountType);
        }
    }
}
