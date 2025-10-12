using GunterBar.Application.Interfaces;
using System.Net.Mail;
using System.Net;
using GunterBar.Application.DTOs;

namespace GunterBar.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["Email:SmtpServer"];
        _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
        _smtpUsername = configuration["Email:Username"];
        _smtpPassword = configuration["Email:Password"];
        _fromEmail = configuration["Email:FromEmail"];
    }

    public async Task SendOrderConfirmationAsync(OrderDto order, string userEmail)
    {
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
        // Implementa la plantilla HTML para el correo de confirmación de orden
        return $@"
            <h1>¡Gracias por tu orden!</h1>
            <p>Tu orden ha sido confirmada.</p>
            <p>Número de orden: {order.Id}</p>
            <!-- Más detalles de la orden -->
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
