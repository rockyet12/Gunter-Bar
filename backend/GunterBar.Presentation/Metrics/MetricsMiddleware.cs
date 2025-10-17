using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GunterBar.Presentation.Metrics;

public class MetricsMiddleware : IMetricsMiddleware
{
    private readonly IMetricCollector _metrics;

    public MetricsMiddleware(IMetricCollector metrics)
    {
        _metrics = metrics;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.Request.Path.Value?.ToLower() ?? "";
        var startTime = DateTime.UtcNow;

        try
        {
            // Registro de usuarios activos
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                _metrics.UpdateActiveUsers(1);
            }

            await next(context);

            // Métricas de latencia de API
            var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
            _metrics.RecordApiLatency(endpoint, elapsed);
        }
        catch (Exception)
        {
            _metrics.IncrementErrorCount(endpoint);
            throw;
        }
        finally
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                _metrics.UpdateActiveUsers(-1);
            }
        }
    }
}
