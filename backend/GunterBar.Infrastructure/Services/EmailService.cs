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
        var subject = "¡Bienvenido a Gunter Bar - 10% de descuento en tu primera compra!";
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
        return $@"
            <!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Bienvenido a Gunter Bar</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .discount-box {{ background: #fff; border: 2px solid #667eea; border-radius: 8px; padding: 20px; margin: 20px 0; text-align: center; }}
                    .discount-code {{ font-size: 24px; font-weight: bold; color: #667eea; margin: 10px 0; }}
                    .cta-button {{ display: inline-block; background: #667eea; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                    .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>¡Bienvenido a Gunter Bar, {userName}!</h1>
                        <p>Tu aventura en el mejor bar comienza ahora</p>
                    </div>
                    <div class='content'>
                        <h2>🎉 ¡Felicitaciones por registrarte!</h2>
                        <p>Estamos emocionados de tenerte como parte de nuestra comunidad. En Gunter Bar, nos apasiona ofrecerte las mejores bebidas y experiencias únicas.</p>

                        <div class='discount-box'>
                            <h3>🎁 ¡Descuento Especial de Bienvenida!</h3>
                            <p>Como regalo de bienvenida, te ofrecemos:</p>
                            <div class='discount-code'>10% OFF</div>
                            <p><strong>En tu primera compra</strong></p>
                            <p>Usa el código: <strong>WELCOME10</strong></p>
                        </div>

                        <h3>¿Qué puedes hacer ahora?</h3>
                        <ul>
                            <li>🌟 Explora nuestro menú de bebidas premium</li>
                            <li>📱 Realiza pedidos desde tu dispositivo</li>
                            <li>⭐ Disfruta de promociones exclusivas</li>
                            <li>🎯 Acumula puntos por tus compras</li>
                        </ul>

                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='#' class='cta-button'>¡Empieza a Explorar!</a>
                        </div>

                        <p>Si tienes alguna pregunta, no dudes en contactarnos. ¡Estamos aquí para hacer que tu experiencia sea increíble!</p>

                        <p>Saludos cordiales,<br>
                        <strong>El equipo de Gunter Bar</strong></p>
                    </div>
                    <div class='footer'>
                        <p>📍 Calle Principal 123, Ciudad<br>
                        📞 +57 300 123 4567<br>
                        ✉️ hola@gunterbar.com</p>
                        <p>Este es un email automático, por favor no respondas directamente.</p>
                    </div>
                </div>
            </body>
            </html>
        ";
    }
}
