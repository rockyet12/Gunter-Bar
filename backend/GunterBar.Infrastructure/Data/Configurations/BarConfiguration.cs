using GunterBar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GunterBar.Infrastructure.Data.Configurations;

public class BarConfiguration : IEntityTypeConfiguration<Bar>
{
    public void Configure(EntityTypeBuilder<Bar> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.Property(b => b.OwnerId)
            .IsRequired();

        builder.Property(b => b.Address)
            .HasMaxLength(500);

        builder.Property(b => b.City)
            .HasMaxLength(100);

        builder.Property(b => b.PostalCode)
            .HasMaxLength(20);

        builder.Property(b => b.Country)
            .HasMaxLength(100);

        builder.Property(b => b.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(b => b.Email)
            .HasMaxLength(255);

        builder.Property(b => b.ImageUrl)
            .HasMaxLength(1000);

        builder.Property(b => b.OpeningHours)
            .HasMaxLength(500);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(b => b.Name)
            .IsUnique();

        builder.HasIndex(b => b.OwnerId)
            .IsUnique(); // Un usuario solo puede tener un bar

        builder.HasOne(b => b.Owner)
            .WithOne(u => u.Bar)
            .HasForeignKey<Bar>(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Drinks)
            .WithOne(d => d.Bar)
            .HasForeignKey(d => d.BarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
