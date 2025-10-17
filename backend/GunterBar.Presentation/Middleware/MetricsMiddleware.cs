using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Claims;
using GunterBar.Presentation.Metrics;

namespace GunterBar.Presentation.Middleware;

public class MetricsMiddleware : IMetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMetricCollector _metrics;

    public MetricsMiddleware(RequestDelegate next, IMetricCollector metrics)
    {
        _next = next;
        _metrics = metrics;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var endpoint = context.Request.Path.ToString();

        try
        {
            await _next(context);

            sw.Stop();
            _metrics.RecordApiLatency(endpoint, sw.ElapsedMilliseconds);
            
            // Incrementar usuarios activos si es un endpoint de login exitoso
            if (endpoint.EndsWith("/login") && context.Response.StatusCode == 200)
            {
                _metrics.UpdateActiveUsers(1);
                _metrics.RecordLoginAttempt(true);
            }
        }
        catch (System.Exception ex)
        {
            _metrics.IncrementErrorCount(ex.GetType().Name);
            throw;
        }
    }
}
