using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GunterBar.Infrastructure.Data;
using GunterBar.Infrastructure.Services;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Repositories;
using GunterBar.Application.Interfaces;

namespace GunterBar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));
        
        services.AddDbContext<GunterBarDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDrinkRepository, DrinkRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();

        // Cache
        services.AddMemoryCache();

        return services;
    }
}
