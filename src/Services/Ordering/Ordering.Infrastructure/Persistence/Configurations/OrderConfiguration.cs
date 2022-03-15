using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.UserName).HasMaxLength(50).IsRequired();
            builder.Property(o => o.TotalPrice).IsRequired();

            // BillingAddress
            builder.Property(o => o.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(o => o.LastName).HasMaxLength(50).IsRequired();
            builder.Property(o => o.EmailAddress).HasMaxLength(50).IsRequired();
            builder.Property(o => o.AddressLine).HasMaxLength(255).IsRequired();
            builder.Property(o => o.Country).HasMaxLength(100).IsRequired();
            builder.Property(o => o.State).HasMaxLength(100).IsRequired();
            builder.Property(o => o.ZipCode).HasMaxLength(50).IsRequired();

            // Payment
            builder.Property(o => o.CardName).HasMaxLength(50).IsRequired();
            builder.Property(o => o.CardNumber).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Expiration).HasMaxLength(50).IsRequired();
            builder.Property(o => o.CVV).HasMaxLength(50).IsRequired();
            builder.Property(o => o.PaymentMethod).IsRequired();
        }
    }
}
