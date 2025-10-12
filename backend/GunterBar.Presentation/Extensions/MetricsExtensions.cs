using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace GunterBar.Presentation.Extensions;

public static class MetricsExtensions
{
    public static IServiceCollection AddCustomMetrics(this IServiceCollection services)
    {
        // Registrar métricas personalizadas
        services.AddSingleton<MetricCollector>();
        return services;
    }

    public static void UseCustomMetrics(this IApplicationBuilder app)
    {
        // Exponer métricas en /metrics
        app.UseMetricServer();
        
        // Middleware para recopilar métricas HTTP
        app.UseHttpMetrics();
    }
}

public class MetricCollector
{
    private readonly Counter _ordersCreated;
    private readonly Counter _drinksAdded;
    private readonly Gauge _activeUsers;
    private readonly Histogram _orderProcessingTime;

    public MetricCollector()
    {
        _ordersCreated = Metrics.CreateCounter("gunterbar_orders_created_total", "Número total de órdenes creadas");
        _drinksAdded = Metrics.CreateCounter("gunterbar_drinks_added_total", "Número total de bebidas agregadas");
        _activeUsers = Metrics.CreateGauge("gunterbar_active_users", "Número de usuarios activos");
        _orderProcessingTime = Metrics.CreateHistogram("gunterbar_order_processing_seconds", "Tiempo de procesamiento de órdenes");
    }

    public void OrderCreated() => _ordersCreated.Inc();
    public void DrinkAdded() => _drinksAdded.Inc();
    public void SetActiveUsers(int count) => _activeUsers.Set(count);
    public ITimer TimeOrderProcessing() => _orderProcessingTime.NewTimer();
}
