using Microsoft.EntityFrameworkCore;
using BarGunter.Infrastructure.Persistences;

var builder = WebApplication.CreateBuilder(args);

var AllowedOrigins = builder.Configuration["AllowedOrigins"] ?? throw new Exception("No hay orígenes permitidos configurados");
var DataBaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("No hay conexion a la base de datos");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.WithOrigins(AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Configura el DbContext con MySQL
builder.Services.AddDbContext<BarGunterDbContext>(options =>
    options.UseMySql(
        DataBaseConnectionString,
        new MySqlServerVersion(new Version(8, 4))
    ));


var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();





app.Run();