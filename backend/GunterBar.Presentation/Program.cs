using System.Text;
using GunterBar.Application;
using GunterBar.Infrastructure;
using GunterBar.Infrastructure.Data;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Repositories;
using GunterBar.Application.Interfaces;
using GunterBar.Application.Services;
using GunterBar.Infrastructure.Services;
using GunterBar.Presentation.Extensions;
using GunterBar.Presentation.Middleware;
using GunterBar.Presentation.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GunterBar.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleware(app);

        // Servir archivos estáticos desde /uploads
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
            RequestPath = "/uploads"
        });

        if (app.Environment.IsDevelopment())
        {
            await InitializeDatabaseAsync(app);
        }

        await app.RunAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Servicios principales
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCustomSwagger();
        
        // Capas de aplicación e infraestructura
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        // Monitoreo y rendimiento
        builder.Services.AddCustomHealthChecks(builder.Configuration);
        builder.Services.AddCustomMetrics();
        builder.Services.AddCustomRateLimiting();

        // Autenticación y autorización
        ConfigureJwtAuthentication(builder);

        // Repositorios
        ConfigureRepositories(builder.Services);

        // Servicios de negocio
        ConfigureBusinessServices(builder.Services);

        // CORS y Cache
        ConfigureCors(builder.Services);
        builder.Services.AddMemoryCache();
    }

    private static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "GunterBar-SecretKey-2025-ET12-Development-Key";

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "GunterBar",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "GunterBar-Users",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDrinkRepository, DrinkRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }

    private static void ConfigureBusinessServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICacheService, CacheService>();
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173", "http://localhost:5174") // Frontend Vite dev server and seller frontend
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Importante: permitir credenciales (cookies)
            });
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Desarrollo
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gunter Bar API v1");
                c.RoutePrefix = "swagger";
            });
        }

        // Seguridad básica
        // app.UseHttpsRedirection(); // Removido para desarrollo con localhost

        // CORS debe ir lo más temprano posible
        app.UseCors("AllowFrontend");

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        // Endpoints
        app.MapControllers();
    }

    private static async Task InitializeDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<GunterBarDbContext>();
            await context.Database.MigrateAsync();
            await DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error al inicializar la base de datos");
        }
    }
}
