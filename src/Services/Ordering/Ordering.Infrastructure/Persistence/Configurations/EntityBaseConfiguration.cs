using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Common;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class EntityBaseConfiguration : IEntityTypeConfiguration<EntityBase>
    {
        public void Configure(EntityTypeBuilder<EntityBase> builder)
        {
            builder.Property(e => e.CreatedBy).HasMaxLength(100);
            builder.Property(o => o.LastModifiedBy).HasMaxLength(100);
        }
    }
}
