using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GunterBar.Presentation.Infrastructure;

public interface IMetricsMiddleware
{
    Task InvokeAsync(HttpContext context);
}
