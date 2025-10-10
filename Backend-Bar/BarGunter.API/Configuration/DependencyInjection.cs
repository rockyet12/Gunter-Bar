namespace BarGunter.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        // Register API-specific services, filters, middlewares etc.
        return services;
    }
}
