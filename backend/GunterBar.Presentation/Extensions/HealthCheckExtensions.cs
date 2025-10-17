using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GunterBar.Presentation.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks();

        var dbConnection = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(dbConnection))
        {
            healthChecks.AddSqlServer(
                connectionString: dbConnection,
                healthQuery: "SELECT 1;",
                name: "Database",
                tags: new[] { "db", "sql", "sqlserver" });
        }

        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            healthChecks.AddRedis(
                redisConnectionString: redisConnection,
                name: "Redis",
                tags: new[] { "cache", "redis" });
        }

        var externalApiUrl = configuration["ExternalServices:ApiUrl"];
        if (!string.IsNullOrEmpty(externalApiUrl))
        {
            healthChecks.AddUrlGroup(
                new Uri(externalApiUrl),
                name: "ExternalAPI",
                tags: new[] { "api", "external" });
        }

        return services;
    }

    public static void UseCustomHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = WriteResponse,
            AllowCachingResponses = false
        });
    }

    private static Task WriteResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = result.Status.ToString(),
            checks = result.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = $"{e.Value.Duration.TotalMilliseconds}ms",
                tags = e.Value.Tags
            }),
            totalDuration = $"{result.TotalDuration.TotalMilliseconds}ms",
            timestamp = DateTime.UtcNow
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
