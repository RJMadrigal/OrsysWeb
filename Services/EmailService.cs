using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services.Interfaces;
using Microsoft.Extensions.Options;
using ServiceStack.Text;

namespace SistemaOrdenes.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;  // Para obtener HttpContext
        private readonly IHostEnvironment _env;  // Para obtener rutas del sistema

        // Constructor con dependencias inyectadas
        public EmailService(IOptions<EmailSettings> emailSettings, IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _httpContextAccessor = httpContextAccessor;  // Inyectar HttpContext
            _env = env;  // Inyectar IHostEnvironment
        }

        public async Task<bool> SendEmail(Email correodto)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                email.To.Add(MailboxAddress.Parse(correodto.Para)); 
                email.Subject = correodto.Asunto;
                email.Body = new TextPart(TextFormat.Html) { Text = correodto.Contenido };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fallo el envio del email: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> SendConfirmationEmail(string Correo, string Nombre, string Token)
        {
            try
            {
                // Obtener HttpContext actual
                var request = _httpContextAccessor.HttpContext.Request;

                // Leer la plantilla HTML desde la carpeta Services/Templates
                string path = System.IO.Path.Combine(_env.ContentRootPath, "Services", "Templates", "Confirmar.html");
                string content = await System.IO.File.ReadAllTextAsync(path);

                // Crear la URL de confirmación
                string url = $"{request.Scheme}://{request.Host}/Usuarios/Confirmar?token={Token}";

                // Reemplazar valores en la plantilla HTML
                string htmlBody = string.Format(content, Nombre, url);

                // Configurar el objeto de correo
                Email correodto = new Email()
                {
                    Para = Correo,
                    Asunto = "Correo de Confirmación",
                    Contenido = htmlBody
                };

                // Enviar el correo
                bool enviado = await SendEmail(correodto);

                return enviado;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al enviar el correo de confirmación: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> SendResetPasswordEmail(string Correo, string Nombre, string Token)
        {
            try
            {
                // Obtener HttpContext actual
                var request = _httpContextAccessor.HttpContext.Request;

                // Leer la plantilla HTML desde la carpeta Services/Templates
                string path = System.IO.Path.Combine(_env.ContentRootPath, "Services", "Templates", "Restablecer.html");
                string content = await System.IO.File.ReadAllTextAsync(path);

                // Crear la URL de confirmación
                string url = $"{request.Scheme}://{request.Host}/Usuarios/ActualizarContraseña?token={Token}";


                // Reemplazar valores en la plantilla HTML
                string htmlBody = string.Format(content, Nombre, url);

                System.Diagnostics.Debug.WriteLine("Correo: " + Correo);

                // Configurar el objeto de correo
                Email correodto = new Email()
                {
                    Para = Correo,
                    Asunto = "Restablecer tu contraseña - Orsys",
                    Contenido = htmlBody
                };

                // Enviar el correo
                bool enviado = await SendEmail(correodto);

                return enviado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al enviar el correo de reset: {ex.Message}");
                return false;
            }
        }


    }


}
