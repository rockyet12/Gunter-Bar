using GunterBar.Application.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using GunterBar.Application.DTOs.Order;

namespace GunterBar.Infrastructure.Services;

public class EmailService : IEmailService
{
    // ...existing code...

    public async Task SendOrderPaidEmailAsync(OrderDto order, string userEmail)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentException("User email is required", nameof(userEmail));

        var subject = "Pago recibido - Gunter Bar";
        var body = GenerateOrderPaidEmail(order);
        await SendEmailAsync(userEmail, subject, body);
    }

    private string GenerateOrderPaidEmail(OrderDto order)
    {
        var userInfo = $@"
            <h2>Información del cliente</h2>
            <ul>
                <li><strong>Nombre:</strong> {order.UserName}</li>
                <li><strong>ID Usuario:</strong> {order.UserId}</li>
                <li><strong>Fecha del pedido:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</li>
            </ul>
        ";

        var itemsInfo = "<h2>Detalle del pedido</h2><ul>";
        foreach (var item in order.Items)
        {
            itemsInfo += $"<li>{item.Quantity} x {item.DrinkName} - ${item.Subtotal:F2}</li>";
        }
        itemsInfo += "</ul>";

        var totalInfo = $"<p><strong>Total pagado:</strong> ${order.Total:F2}</p>";

        var thanks = "<h3>¡Gracias por tu pago!</h3><p>Tu compra ha sido procesada exitosamente. Pronto recibirás tu pedido.</p>";

        return $@"
            <h1>Confirmación de pago</h1>
            <p>Hemos recibido tu pago y tu orden está siendo preparada.</p>
            <p>Número de orden: {order.Id}</p>
            {userInfo}
            {itemsInfo}
            {totalInfo}
            {thanks}
        ";
    }
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    public EmailService(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        _smtpServer = configuration["Email:SmtpServer"] ?? 
            throw new InvalidOperationException("SMTP Server not configured");
        
        _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "587");
        
        _smtpUsername = configuration["Email:Username"] ?? 
            throw new InvalidOperationException("SMTP Username not configured");
        
        _smtpPassword = configuration["Email:Password"] ?? 
            throw new InvalidOperationException("SMTP Password not configured");
        
        _fromEmail = configuration["Email:FromEmail"] ?? 
            throw new InvalidOperationException("From Email not configured");
    }

    public async Task SendOrderConfirmationAsync(OrderDto order, string userEmail)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
            
        if (string.IsNullOrWhiteSpace(userEmail))
            throw new ArgumentException("User email is required", nameof(userEmail));

        var subject = "Confirmación de Orden - Gunter Bar";
        var body = GenerateOrderConfirmationEmail(order);
        await SendEmailAsync(userEmail, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string resetToken)
    {
        var subject = "Restablecer Contraseña - Gunter Bar";
        var body = GeneratePasswordResetEmail(resetToken);
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendWelcomeEmailAsync(string email, string userName)
    {
        var subject = "Bienvenido a Gunter Bar";
        var body = GenerateWelcomeEmail(userName);
        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(to);

        await client.SendMailAsync(message);
    }

    private string GenerateOrderConfirmationEmail(OrderDto order)
    {
        // Plantilla HTML mejorada con información del usuario y agradecimiento
        var userInfo = $@"
            <h2>Información del cliente</h2>
            <ul>
                <li><strong>Nombre:</strong> {order.UserName}</li>
                <li><strong>ID Usuario:</strong> {order.UserId}</li>
                <li><strong>Fecha del pedido:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</li>
            </ul>
        ";

        var itemsInfo = "<h2>Detalle del pedido</h2><ul>";
        foreach (var item in order.Items)
        {
            itemsInfo += $"<li>{item.Quantity} x {item.DrinkName} - ${item.Subtotal:F2}</li>";
        }
        itemsInfo += "</ul>";

        var totalInfo = $"<p><strong>Total:</strong> ${order.Total:F2}</p>";

        var thanks = "<h3>¡Gracias por tu compra en Gunter Bar!</h3><p>Esperamos que disfrutes tu pedido. Si tienes dudas, responde a este correo.</p>";

        return $@"
            <h1>Confirmación de tu pedido</h1>
            <p>Tu orden ha sido confirmada y está en proceso.</p>
            <p>Número de orden: {order.Id}</p>
            {userInfo}
            {itemsInfo}
            {totalInfo}
            {thanks}
        ";
    }

    private string GeneratePasswordResetEmail(string resetToken)
    {
        // Implementa la plantilla HTML para el correo de restablecimiento de contraseña
        return $@"
            <h1>Restablecer Contraseña</h1>
            <p>Has solicitado restablecer tu contraseña.</p>
            <p>Usa el siguiente token: {resetToken}</p>
        ";
    }

    private string GenerateWelcomeEmail(string userName)
    {
        // Implementa la plantilla HTML para el correo de bienvenida
        return $@"
            <h1>¡Bienvenido a Gunter Bar, {userName}!</h1>
            <p>Gracias por registrarte en nuestro servicio.</p>
        ";
    }
}
