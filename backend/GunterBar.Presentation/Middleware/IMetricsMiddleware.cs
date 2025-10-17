using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Claims;

namespace GunterBar.Presentation.Middleware;

public interface IMetricsMiddleware
{
    Task InvokeAsync(HttpContext context);
}
