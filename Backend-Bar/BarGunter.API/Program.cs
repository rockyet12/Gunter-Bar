using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BarGunter.Infrastructure.Persistences;
using BarGunter.Infrastructure.Persistences.Repositories;
using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Application.Services;

try
{
    Console.WriteLine("[START] Program.cs starting");
    var builder = WebApplication.CreateBuilder(args);

    var allowedOrigins = builder.Configuration["AllowedOrigins"];
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarGunter API", Version = "v1" });
    });
    
    builder.Services.AddControllers();

    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        builder.Services.AddDbContext<BarGunterDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4))));
    }

    // DI registrations - English service names
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
    builder.Services.AddScoped<IDrinkService, DrinkService>();
    builder.Services.AddScoped<ICartRepository, CartRepository>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<ITicketRepository, TicketRepository>();
    builder.Services.AddScoped<ITicketService, TicketService>();
    builder.Services.AddScoped<IDrinkTypeRepository, DrinkTypeRepository>();
    builder.Services.AddScoped<IDrinkTypeService, DrinkTypeService>();

    // JWT authentication removed since Usuario system is removed

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            if (!string.IsNullOrWhiteSpace(allowedOrigins))
            {
                var origins = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }
            else
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
        });
    });

    var app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseRouting();
    
    // Authentication removed since Usuario system is removed
    app.UseAuthorization();
    app.UseCors("AllowAllOrigins");
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("[FATAL] Startup exception: " + ex);
    throw;
}
