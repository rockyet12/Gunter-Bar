using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Claims;


namespace GunterBar.Presentation.Infrastructure;

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
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                _metrics.UpdateActiveUsers(1);
            }

            await _next(context);
        }
        catch (System.Exception ex)
        {
            _metrics.IncrementErrorCount(ex.GetType().Name);
            throw;
        }
        finally
        {
            sw.Stop();
            _metrics.RecordApiLatency(endpoint, sw.ElapsedMilliseconds);

            // Registrar errores basados en el código de estado HTTP
            var statusCode = context.Response.StatusCode;
            if (statusCode >= 400)
            {
                _metrics.IncrementErrorCount($"HTTP{statusCode}");
            }

            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                _metrics.UpdateActiveUsers(-1);
            }
        }
    }
}
