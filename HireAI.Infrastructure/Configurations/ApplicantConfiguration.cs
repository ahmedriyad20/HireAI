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


            // Navigation properties
            builder.HasMany(a => a.ApplicantSkills)
                .WithOne(asn => asn.Applicant)
                .HasForeignKey(asn => asn.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Navigation properties
        builder
      .HasOne(a => a.CV)
      .WithOne(c => c.Applicant)
      .HasForeignKey<CV>(c => c.ApplicantId);   // CV is the dependent
     

            builder.HasMany(a => a.Applications)
                .WithOne(app => app.Applicant)
                .HasForeignKey(app => app.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Exams)
                .WithOne(e => e.Applicant)
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        
        }
    }
}
