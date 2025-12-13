using HireAI.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversityHousingSystem.Infrastructure.Config.Identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(u => u.ResetCode)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(2000)
                .IsRequired(false);
            builder.HasOne(u => u.Applicant).WithOne().OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(u => u.HR).WithOne().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
