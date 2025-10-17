using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GunterBar.Presentation.Metrics;

public interface IMetricsMiddleware
{
    Task InvokeAsync(HttpContext context, RequestDelegate next);
}
