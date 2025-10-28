using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Claims;
using GunterBar.Presentation.Metrics;

namespace GunterBar.Presentation.Middleware;

public class MetricsMiddleware
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

            // Registro de usuarios activos
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                _metrics.UpdateActiveUsers(1);
            }
            
            // Registrar errores basados en el código de estado HTTP
            var statusCode = context.Response.StatusCode;
            if (statusCode >= 400)
            {
                var errorType = statusCode switch
                {
                    400 => "bad_request",
                    401 => "unauthorized",
                    403 => "forbidden",
                    404 => "not_found",
                    405 => "method_not_allowed",
                    500 => "internal_error",
                    _ => "other"
                };
                _metrics.IncrementErrorCount(errorType);
            }
            
            // Métricas de login
            if (endpoint.EndsWith("/login"))
            {
                _metrics.RecordLoginAttempt(statusCode == 200);
            }
        }
    }
}
