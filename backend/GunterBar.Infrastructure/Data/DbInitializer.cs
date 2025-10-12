using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;

namespace GunterBar.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task Initialize(GunterBarDbContext context)
    {
        // Asegurarse de que la base de datos está creada
        await context.Database.EnsureCreatedAsync();

        // Verificar si ya hay datos
        if (await context.Users.AnyAsync())
            return; // DB ya ha sido inicializada

        // Crear usuario admin por defecto
        var adminUser = new User
        {
            Name = "Admin",
            Email = "admin@gunterbar.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = UserRole.Admin
        };
        await context.Users.AddAsync(adminUser);

        // Crear algunas bebidas iniciales
        var drinks = new List<Drink>
        {
            new Drink
            {
                Name = "Mojito",
                Description = "Bebida cubana a base de ron, limón y menta",
                Price = 8.99m,
                Type = DrinkType.Cocktail,
                ImageUrl = "/images/drinks/mojito.jpg",
                IsAvailable = true,
                Stock = 100
            },
            new Drink
            {
                Name = "Margarita",
                Description = "Cóctel mexicano con tequila, triple sec y limón",
                Price = 9.99m,
                Type = DrinkType.Cocktail,
                ImageUrl = "/images/drinks/margarita.jpg",
                IsAvailable = true,
                Stock = 100
            }
        };
        await context.Drinks.AddRangeAsync(drinks);

        await context.SaveChangesAsync();
    }
}
