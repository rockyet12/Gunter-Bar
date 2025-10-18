using GunterBar.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GunterBar.Infrastructure.Data.Configurations;

public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime");
    }
}
