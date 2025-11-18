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

            // Foreign Key
            builder.HasOne(asn => asn.Applicant)
                .WithMany(a => a.ApplicantSkills)
                .HasForeignKey(asn => asn.ApplicantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many relationship with Skills
            builder.HasMany(asn => asn.Skills)
                .WithMany()
                .UsingEntity("ApplicantSkillJoin");

            // Index
            builder.HasIndex(asn => asn.ApplicantId);

            // Check constraint
            builder.ToTable(t => t.HasCheckConstraint("CK_ApplicantSkill_Rate", "([SkillRate] >= 0 AND [SkillRate] <= 100) OR [SkillRate] IS NULL"));
        }
    }
}
