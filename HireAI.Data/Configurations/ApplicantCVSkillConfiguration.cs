using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class ApplicantCVSkillConfiguration : IEntityTypeConfiguration<ApplicantCVSkill>
    {
        public void Configure(EntityTypeBuilder<ApplicantCVSkill> builder)
        {
            builder.HasKey(acs => acs.Id);

            builder.Property(acs => acs.Name)
                .HasMaxLength(100);

            builder.Property(acs => acs.CVId)
                .IsRequired();

            // Index
            builder.HasIndex(acs => acs.CVId);
        }
    }
}
