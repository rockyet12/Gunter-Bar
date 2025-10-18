using System;
using System.IO;
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

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));
        
        var optionsBuilder = new DbContextOptionsBuilder<GunterBarDbContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new GunterBarDbContext(optionsBuilder.Options);
    }
}
