using HireAI.Data.Helpers.Enums;
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

            #region Type Conversion
            builder.Property(u => u.Status)
                  .HasConversion(
                v => v.ToString(),// Converts the enum to string when saving to the database                  
               v => (enPaymentStatus)Enum.Parse(typeof(enPaymentStatus), v)// Converts the string back to enum when reading from the database
                );

            builder.Property(u => u.UpgradeTo)
                 .HasConversion(
               v => v.ToString(),
              v => (enAccountType)Enum.Parse(typeof(enAccountType), v)
               );

            builder.Property(u => u.BillingPeriod)
                .HasConversion(
              v => v.ToString(),               
             v => (enBillingPeriod)Enum.Parse(typeof(enBillingPeriod), v)
              );
            #endregion

            // Unique constraint for PaymentIntentId
            builder.HasIndex(p => p.PaymentIntentId)
                .IsUnique();

            // Unique constraint for HrId and CreatedAt combination
            builder.HasIndex(p => new { p.HrId, p.CreatedAt });

            // Add check constraint for positive amount
            builder.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", "[Amount] > 0"));

            // Add validation for currency code
            builder.ToTable(t => t.HasCheckConstraint("CK_Payment_Currency", "LEN([Currency]) = 3"));
        }
    }
}
