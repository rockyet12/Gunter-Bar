using Microsoft.EntityFrameworkCore;
using BarGunter.Infrastructure.Persistences;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Services;
using BarGunter.Infrastructure.Persistences.Repositories;
using BarGunter.Application.Contracts.IServices;


var builder = WebApplication.CreateBuilder(args);

//---------------------Leer variables de entorno----------------------------------------------
var AllowedOrigins = builder.Configuration["AllowedOrigins"] ?? throw new Exception("No hay orígenes permitidos configurados");
var DataBaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("No hay conexion a la base de datos");


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//---------------------Configuracion de la base de datos---------------------------------------------------
builder.Services.AddDbContext<BarGunterDbContext>(options =>
    options.UseMySql(
        DataBaseConnectionString,
        new MySqlServerVersion(new Version(8, 4))
    ));


//---------------------Inyeccion de dependencias------------------------------------------------
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// Es importante que el nombre de la clase de implementación sea "UsuarioService" y no "UsuarioService"
builder.Services.AddScoped<ITragoRepository, TragoRepository>();
builder.Services.AddScoped<ITragoService, TragoService>();


//---------------------JWT Authentication------------------------------------------------------------------------------
var key = Encoding.ASCII.GetBytes("TucodigodeseguridadWAZAAAAAAA!!");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x=>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.WithOrigins(AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//------------------------------------------------------------------------------------------------------------
app.UseHttpsRedirection();
app.UseRouting();

// Habilitar la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");
app.MapControllers();


app.Run();