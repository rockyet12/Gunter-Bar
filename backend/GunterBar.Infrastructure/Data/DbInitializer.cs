using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;

namespace GunterBar.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task Initialize(GunterBarDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Users.AnyAsync())
            return; // DB has already been initialized

        // Create default admin user with secure password
        var defaultAdminPassword = Environment.GetEnvironmentVariable("ADMIN_DEFAULT_PASSWORD") ?? "Admin123!"; // TODO: Remove fallback in production
        var adminUser = new User(
            name: "Admin",
            email: "admin@gunterbar.com",
            passwordHash: BCrypt.Net.BCrypt.HashPassword(defaultAdminPassword)
        )
        {
            Role = UserRole.Vendor
        };
        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync(); // Save to get Id

        // Create a bar for the admin
        var adminBar = new Bar
        {
            Name = "Gunter Bar",
            Description = "Default bar for admin",
            Address = "Default Address",
            OwnerId = adminUser.Id
        };
        await context.Bars.AddAsync(adminBar);
        await context.SaveChangesAsync();

        // Create initial drinks with their ingredients
        var mojito = new Drink("Mojito", 8.99m, 100, DrinkType.Cocktail, "Cuban cocktail with rum, lime, and mint")
        {
            ImageUrl = "/images/drinks/mojito.jpg",
            IsAvailable = true,
            BarId = adminBar.Id
        };
        
        await context.Drinks.AddAsync(mojito);
        await context.SaveChangesAsync(); // Save to get Id
        
        mojito.Ingredients.Add(new DrinkIngredient(mojito.Id, "White Rum", 50));
        mojito.Ingredients.Add(new DrinkIngredient(mojito.Id, "Fresh Lime Juice", 30));
        mojito.Ingredients.Add(new DrinkIngredient(mojito.Id, "Mint Leaves", 6));
        mojito.Ingredients.Add(new DrinkIngredient(mojito.Id, "Sugar Syrup", 20));
        mojito.Ingredients.Add(new DrinkIngredient(mojito.Id, "Soda Water", 100));

        var margarita = new Drink("Margarita", 9.99m, 100, DrinkType.Cocktail, "Mexican cocktail with tequila, triple sec, and lime")
        {
            ImageUrl = "/images/drinks/margarita.jpg",
            IsAvailable = true,
            BarId = adminBar.Id
        };
        
        await context.Drinks.AddAsync(margarita);
        await context.SaveChangesAsync();
        
        margarita.Ingredients.Add(new DrinkIngredient(margarita.Id, "Tequila", 50));
        margarita.Ingredients.Add(new DrinkIngredient(margarita.Id, "Triple Sec", 25));
        margarita.Ingredients.Add(new DrinkIngredient(margarita.Id, "Fresh Lime Juice", 25));
        margarita.Ingredients.Add(new DrinkIngredient(margarita.Id, "Salt", 1));

        await context.SaveChangesAsync();
    }
}
