using SistemaOrdenes.Models;

namespace SistemaOrdenes.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
        Task<bool> SendConfirmationEmail(string Correo, string Nombre, string Token);
        Task<bool> SendResetPasswordEmail(string Correo, string Nombre, string Token);
    }
}
