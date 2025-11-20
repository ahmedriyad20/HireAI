using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicantSkillConfiguration : IEntityTypeConfiguration<ApplicantSkill>
    {
        public void Configure(EntityTypeBuilder<ApplicantSkill> builder)
        {
            builder.HasKey(asn => asn.Id);

            builder.Property(asn => asn.SkillRate)
                .IsRequired(false);

            // Applicant (many ApplicantSkill per Applicant)
            builder.HasOne(asn => asn.Applicant)
                .WithMany(a => a.ApplicantSkills)
                .HasForeignKey(asn => asn.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Skill (many ApplicantSkill per Skill) - bridge FK
            builder.HasOne(asn => asn.Skill)
                .WithMany(s => s.ApplicantSkills)
                .HasForeignKey(asn => asn.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(asn => asn.ApplicantId);
            builder.HasIndex(asn => asn.SkillId);

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_ApplicantSkill_Rate", "([SkillRate] >= 0 AND [SkillRate] <= 100) OR [SkillRate] IS NULL"));
        }
    }
}
