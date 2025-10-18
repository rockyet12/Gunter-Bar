
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prometheus;

namespace GunterBar.Presentation.Infrastructure;

public static class MetricsExtensions
{
    public static IServiceCollection AddCustomMetrics(this IServiceCollection services)
    {
        // Registrar la configuración por defecto si no está configurada
        services.TryAddSingleton<IMetricsConfiguration, DefaultMetricsConfiguration>();
        
        // Registrar el listener por defecto en desarrollo
        services.TryAddSingleton<IMetricsEventListener, ConsoleMetricsListener>();
        
        // Registrar los servicios principales de métricas
        services.AddSingleton<IMetricCollector, MetricCollector>();
        
        return services;
    }

    public static IApplicationBuilder UseCustomMetrics(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IMetricsConfiguration>();

        // Configurar middleware de métricas HTTP detalladas
        app.UseHttpMetrics(options =>
        {
            foreach (var label in configuration.DefaultLabels)
            {
                options.AddCustomLabel(label.Key, _ => label.Value);
            }
            options.ReduceStatusCodeCardinality();
        });

        // Middleware personalizado para métricas de negocio
        app.UseMiddleware<MetricsMiddleware>();
        
        // Configurar endpoint de métricas - esto debe ser lo último
        app.UseMetricServer(configuration.MetricsEndpoint);

        return app;
    }
}
