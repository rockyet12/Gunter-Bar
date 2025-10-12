using GunterBar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.Property(d => d.Type)
            .IsRequired();

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(2048);

        builder.HasMany(d => d.Ingredients)
            .WithOne()
            .HasForeignKey(di => di.DrinkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
