using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GunterBar.Domain.Entities;

namespace GunterBar.Infrastructure.Data.Configurations;

public class DrinkIngredientConfiguration : IEntityTypeConfiguration<DrinkIngredient>
{
    public void Configure(EntityTypeBuilder<DrinkIngredient> builder)
    {
        builder.HasKey(di => di.Id);

        builder.Property(di => di.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(di => di.Quantity)
            .IsRequired()
            .HasDefaultValue(0);
    }
}
