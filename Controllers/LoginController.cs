using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Security.Claims;
using SistemaOrdenes.Services;
using SistemaOrdenes.Models;
using static SistemaOrdenes.Services.EmailService;


namespace SistemaOrdenes.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRepositorioUsuarios _usuarioData;
        
        public LoginController(IRepositorioUsuarios usuarioData)
        {
            _usuarioData = usuarioData;
        }

        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Correo, string Clave)

        {
            try
            {
                var usuario = await _usuarioData.ObtenerUsuarioPorCredenciales(Correo, HashSHA256.CSHA256(Clave));

                if (usuario != null)
                {
                    return  RedirectToAction("Index", "Home");
                }
                else  
                {
                    ViewBag.Message = "Nombre de usuario o contraseña incorrectos";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Console.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
                return View();
            }
        }

    }
}
