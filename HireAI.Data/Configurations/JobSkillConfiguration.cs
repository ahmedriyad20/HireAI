using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
    {
        public void Configure(EntityTypeBuilder<JobSkill> builder)
        {
            builder.HasKey(js => js.Id);

            // Foreign Keys
            builder.HasOne(js => js.Job)
                .WithMany(j => j.JobSkills)
                .HasForeignKey(js => js.JobId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(js => js.Skill)
                .WithMany()
                .HasForeignKey(js => js.SkillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Composite unique constraint to prevent duplicate skill assignment to same job
            builder.HasIndex(js => new { js.JobId, js.SkillId })
                .IsUnique();

            // Index for finding skills by job
            builder.HasIndex(js => js.JobId);
        }
    }
}
