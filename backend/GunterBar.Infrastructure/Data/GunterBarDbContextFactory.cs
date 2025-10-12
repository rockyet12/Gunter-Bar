using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GunterBar.Infrastructure.Data;

public class GunterBarDbContextFactory : IDesignTimeDbContextFactory<GunterBarDbContext>
{
    public GunterBarDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<GunterBarDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new GunterBarDbContext(optionsBuilder.Options);
    }
}
