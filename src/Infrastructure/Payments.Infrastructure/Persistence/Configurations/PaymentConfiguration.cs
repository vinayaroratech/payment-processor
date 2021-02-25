using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(t => t.CreditCardNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(t => t.CardHolder)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.ExpirationDate)
               .IsRequired();

            builder.Property(t => t.SecurityCode)
               .HasMaxLength(3);

            builder.Property(t => t.Amount)
               .IsRequired();

            builder
                .OwnsOne(b => b.Status);
        }
    }
}
