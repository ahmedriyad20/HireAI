using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();


            builder.Property(s => s.Title)
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            // Bridge relationship: one Skill -> many ApplicantSkill entries
            builder.HasMany(s => s.ApplicantSkills)
                .WithOne(asn => asn.Skill)
                .HasForeignKey(asn => asn.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint for Title
            builder.HasIndex(s => s.Title)
                .IsUnique();
        }
    }
}
