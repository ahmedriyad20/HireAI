using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HireAI.Data.Configurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.Property(a => a.ResumeUrl)
                .HasColumnType("varchar(200)");


            //Type Conversion
            builder.Property(u => u.SkillLevel)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToString() : null, // enum? -> string (null preserved)
                    v => string.IsNullOrEmpty(v) ? (enSkillLevel?)null : (enSkillLevel)Enum.Parse(typeof(enSkillLevel), v) // string -> enum?
                );

            // Navigation properties
            builder.HasMany(a => a.ApplicantSkills)
                .WithOne(asn => asn.Applicant)
                .HasForeignKey(asn => asn.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Navigation properties
                builder
              .HasOne(a => a.CV)
              .WithOne(c => c.Applicant)
              .HasForeignKey<CV>(c => c.ApplicantId);   // CV is the dependent
     

            builder.HasMany(a => a.Applications)
                .WithOne(app => app.Applicant)
                .HasForeignKey(app => app.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);
        
        }
    }
}
