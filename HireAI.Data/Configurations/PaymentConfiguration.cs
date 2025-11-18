using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireAI.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentIntentId)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasDefaultValue("USD");

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.CompletedAt)
                .IsRequired(false);

            builder.Property(p => p.UpgradeTo)
                .IsRequired();

            builder.Property(p => p.BillingPeriod)
                .IsRequired();

            // Foreign Key
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for PaymentIntentId
            builder.HasIndex(p => p.PaymentIntentId)
                .IsUnique();

            // Unique constraint for UserId and CreatedAt combination
            builder.HasIndex(p => new { p.UserId, p.CreatedAt });

            // Add check constraint for positive amount
            builder.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", "[Amount] > 0"));

            // Add validation for currency code
            builder.ToTable(t => t.HasCheckConstraint("CK_Payment_Currency", "LEN([Currency]) = 3"));
        }
    }
}
