using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prometheus;
using GunterBar.Presentation.Middleware;
using GunterBar.Presentation.Metrics;

namespace GunterBar.Presentation.Extensions;

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
        services.AddSingleton<GunterBar.Presentation.Middleware.IMetricsMiddleware, GunterBar.Presentation.Middleware.MetricsMiddleware>();
        
        return services;
    }

    public static void UseCustomMetrics(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IMetricsConfiguration>();
        
        // Configurar endpoint de métricas
        app.UseMetricServer(configuration.MetricsEndpoint);
        
        // Middleware para métricas HTTP detalladas
        app.UseHttpMetrics(options =>
        {
            foreach (var label in configuration.DefaultLabels)
            {
                options.AddCustomLabel(label.Key, _ => label.Value);
            }
            options.ReduceStatusCodeCardinality();
        });

        // Middleware personalizado para métricas de negocio
        app.UseMiddleware<GunterBar.Presentation.Middleware.MetricsMiddleware>();
    }
}

public class MetricCollector : IMetricCollector
{
    private readonly Counter _ordersCreated;
    private readonly Counter _drinksAdded;
    private readonly Gauge _activeUsers;
    private readonly Histogram _orderProcessingTime;
    private readonly Histogram _cartValue;
    private readonly Histogram _apiLatency;
    private readonly Counter _loginAttempts;
    private readonly Counter _errorCount;

    public MetricCollector()
    {
        // Métricas de negocio
        _ordersCreated = Prometheus.Metrics.CreateCounter(
            "gunterbar_orders_total",
            "Total de órdenes creadas",
            new CounterConfiguration { LabelNames = new[] { "status" } }
        );

        _drinksAdded = Prometheus.Metrics.CreateCounter(
            "gunterbar_drinks_added_total",
            "Total de bebidas agregadas al inventario",
            new CounterConfiguration { LabelNames = new[] { "category" } }
        );

        _cartValue = Prometheus.Metrics.CreateHistogram(
            "gunterbar_cart_value_dollars",
            "Valor del carrito en dólares",
            new HistogramConfiguration
            {
                Buckets = new[] { 5.0, 10.0, 25.0, 50.0, 100.0, 250.0, 500.0 }
            }
        );

        // Métricas operacionales
        _activeUsers = Prometheus.Metrics.CreateGauge(
            "gunterbar_active_users",
            "Número actual de usuarios activos"
        );

        _orderProcessingTime = Prometheus.Metrics.CreateHistogram(
            "gunterbar_order_processing_seconds",
            "Tiempo de procesamiento de órdenes",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.1, 2, 10)
            }
        );

        _apiLatency = Prometheus.Metrics.CreateHistogram(
            "gunterbar_api_latency_milliseconds",
            "Latencia de endpoints de la API",
            new HistogramConfiguration
            {
                LabelNames = new[] { "endpoint" },
                Buckets = new[] { 10.0, 25.0, 50.0, 100.0, 250.0, 500.0, 1000.0, 2500.0, 5000.0 }
            }
        );

        // Métricas de seguridad
        _loginAttempts = Prometheus.Metrics.CreateCounter(
            "gunterbar_login_attempts_total",
            "Total de intentos de login",
            new CounterConfiguration { LabelNames = new[] { "success" } }
        );

        _errorCount = Prometheus.Metrics.CreateCounter(
            "gunterbar_errors_total",
            "Total de errores por tipo",
            new CounterConfiguration { LabelNames = new[] { "type" } }
        );
    }

    public void OrderCreated(string status) => _ordersCreated.WithLabels(status).Inc();
    public void DrinkAdded(string category) => _drinksAdded.WithLabels(category).Inc();
    public void UpdateActiveUsers(int delta) => _activeUsers.Inc(delta);
    Prometheus.ITimer IMetricCollector.BeginOrderProcessing() => _orderProcessingTime.NewTimer();
    public void RecordCartValue(decimal value) => _cartValue.Observe((double)value);
    public void RecordApiLatency(string endpoint, double milliseconds) => 
        _apiLatency.WithLabels(endpoint).Observe(milliseconds);
    public void RecordLoginAttempt(bool success) => 
        _loginAttempts.WithLabels(success.ToString().ToLower()).Inc();
    public void IncrementErrorCount(string type) => _errorCount.WithLabels(type).Inc();
}
