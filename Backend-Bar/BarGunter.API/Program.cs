using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BarGunter.Infrastructure.Data;
using BarGunter.Infrastructure.Repositories;
using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Application.Interfaces.IServices;
using BarGunter.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CONFIGURACIÓN DE SERVICIOS
// ==========================================
Console.WriteLine("[INFO] Configurando servicios de aplicación...");

// Servicios básicos
builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();

// Controladores con JSON configurado
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ==========================================
// CONFIGURACIÓN DE BASE DE DATOS
// ==========================================
Console.WriteLine("[INFO] Configurando conexión a base de datos...");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Database=BarGunter;User=root;Password=;";

builder.Services.AddDbContext<BarGunterDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

Console.WriteLine("✓ Base de datos MySQL configurada");

// ==========================================
// CONFIGURACIÓN DE REPOSITORIOS Y SERVICIOS
// ==========================================
Console.WriteLine("[INFO] Registrando repositorios y servicios...");

// Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<IDrinkTypeRepository, DrinkTypeRepository>();

// Servicios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IDrinkTypeService, DrinkTypeService>();

Console.WriteLine("✓ Repositorios y servicios registrados");

// ==========================================
// CONFIGURACIÓN DE AUTENTICACIÓN JWT
// ==========================================
Console.WriteLine("[INFO] Configurando autenticación JWT...");

var jwtKey = builder.Configuration["Jwt:Key"] ?? "MiClaveSecretaSuperSeguraQueDebeSerMuyLargaParaJWT123456789";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "BarGunterAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "BarGunterUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
Console.WriteLine("✓ Autenticación JWT configurada");

// ==========================================
// CONFIGURACIÓN DE SWAGGER
// ==========================================
Console.WriteLine("[INFO] Configurando documentación API (Swagger)...");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "BarGunter API", 
        Version = "v1",
        Description = "API para el sistema de gestión del Bar Gunter"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
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
            new string[] {}
        }
    });
});

Console.WriteLine("✓ Swagger configurado con autenticación JWT");

// ==========================================
// CONFIGURACIÓN DE CORS
// ==========================================
Console.WriteLine("[INFO] Configurando CORS...");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

Console.WriteLine("✓ CORS configurado");

// ==========================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ==========================================
var app = builder.Build();

Console.WriteLine("[INFO] Configurando pipeline de middleware...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BarGunter API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
    Console.WriteLine("✓ Swagger UI habilitado en modo desarrollo");
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("✓ Pipeline de middleware configurado");

// ==========================================
// INFORMACIÓN DE ENDPOINTS DISPONIBLES
// ==========================================
Console.WriteLine("==========================================");
Console.WriteLine("🎯 ENDPOINTS DISPONIBLES:");
Console.WriteLine("==========================================");
Console.WriteLine("📋 Swagger UI: http://localhost:5000/");
Console.WriteLine("👤 Users: GET/POST http://localhost:5000/api/User");
Console.WriteLine("🛒 Products: GET/POST http://localhost:5000/api/Product");
Console.WriteLine("📂 Categories: GET/POST http://localhost:5000/api/Category");
Console.WriteLine("🛍️ Cart: GET/POST http://localhost:5000/api/Cart");
Console.WriteLine("📦 Orders: GET/POST http://localhost:5000/api/Order");
Console.WriteLine("🍹 Drinks: GET/POST http://localhost:5000/api/Drink");
Console.WriteLine("🏷️ DrinkTypes: GET/POST http://localhost:5000/api/DrinkType");
Console.WriteLine("🧾 Tickets: GET/POST http://localhost:5000/api/Ticket");
Console.WriteLine("==========================================");
Console.WriteLine("🚀 ¡API lista para recibir peticiones!");
Console.WriteLine("==========================================");

app.Run();
