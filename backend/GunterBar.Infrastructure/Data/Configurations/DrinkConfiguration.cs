using GunterBar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GunterBar.Infrastructure.Data.Configurations;

public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
{
    public void Configure(EntityTypeBuilder<Drink> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.Category)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Sin categoría");

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(2048);

        // Configuración de la relación con DrinkIngredient
        builder.HasMany(d => d.Ingredients)
            .WithOne(di => di.Drink)
            .HasForeignKey(di => di.DrinkId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
