using System.Threading.Tasks;
using GunterBar.Application.DTOs.Order;

namespace GunterBar.Application.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(OrderDto order, string userEmail);
    Task SendOrderPaidEmailAsync(OrderDto order, string userEmail);
    Task SendPasswordResetAsync(string email, string resetToken);
    Task SendWelcomeEmailAsync(string email, string userName);
}
