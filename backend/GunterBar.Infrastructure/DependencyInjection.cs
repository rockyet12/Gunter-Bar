using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GunterBar.Infrastructure.Data;
using GunterBar.Infrastructure.Services;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Repositories;

namespace GunterBar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<GunterBarDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDrinkRepository, DrinkRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();

        // Cache
        services.AddMemoryCache();

        return services;
    }
}
