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
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GunterBar.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de la aplicación y la infraestructura
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Health Checks, Métricas y Rate Limiting
builder.Services.AddCustomHealthChecks(builder.Configuration);
builder.Services.AddCustomMetrics();
builder.Services.AddCustomRateLimiting();        // Configuración de JWT
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

        // Inyección de dependencias - Repositorios
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Inyección de dependencias - Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// Configurar cache en memoria
builder.Services.AddMemoryCache();        // Configuración de Controllers
        builder.Services.AddControllers();

        // Configuración de CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Configuración de Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Gunter Bar API",
                Version = "v1",
                Description = "API para el sistema de gestión de Gunter Bar - ET12",
                Contact = new OpenApiContact
                {
                    Name = "Equipo ET12",
                    Email = "info@et12.edu.ar"
                }
            });

            // Configuración de JWT en Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando Bearer scheme. Ejemplo: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

        // Configuración del pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gunter Bar API v1");
                c.RoutePrefix = "swagger";
            });
        }

app.UseHttpsRedirection();

// Middleware personalizado
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Métricas, Health Checks y Rate Limiting
app.UseCustomMetrics();
app.UseCustomHealthChecks();
app.UseCustomRateLimiting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();        // Ejecutar migraciones automáticamente en desarrollo
if (app.Environment.IsDevelopment())
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
}        app.Run();
    }
}
