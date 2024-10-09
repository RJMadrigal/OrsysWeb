using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Security.Claims;
using SistemaOrdenes.Services;
using SistemaOrdenes.Models;
using static SistemaOrdenes.Services.EmailService;
using System.Diagnostics;
using SistemaOrdenes.Services.Interfaces;


namespace SistemaOrdenes.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRepositorioUsuarios _usuarioData;
        private readonly UsuarioService usuarioService;
        private readonly IEmailService emailService;

        public LoginController(IRepositorioUsuarios usuarioData, UsuarioService usuarioService, IEmailService emailService)
        {
            _usuarioData = usuarioData;
            this.usuarioService = usuarioService;
            this.emailService = emailService;
        }

        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginViewModel modelo)
        {

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            //SE OBTIENE EL USUARIO
            var usuario = await _usuarioData.ObtenerUsuarioPorCredenciales(modelo.Correo, HashSHA256.CSHA256(modelo.Clave));

            //SE VALIDA QUE NO SEA NULO
            if (usuario != null)
            {
                return  RedirectToAction("Index", "Home");
            }
            else  
            {
                ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrecto");
                return View(modelo);
            }
                
        }

        public IActionResult Restablecer()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> Restablecer(RestablecerViewModel modelo /*[FromServices] IWebHostEnvironment env*/)
        {

            //SE OBTIENE EL USUARIO MEDIANTE EL CORREO
            var usuario = await _usuarioData.Obtener(modelo.Correo);

            ViewBag.Correo = modelo.Correo;
            if (usuario != null)
            {
                bool respuesta = await usuarioService.RestablecerActualizarAsync(true, usuario.Clave, usuario.Token);
                if (respuesta)
                {
                    Debug.WriteLine("Correo: " + usuario.Correo);
                    bool enviado = await emailService.SendResetPasswordEmail(modelo.Correo, usuario.Nombre, usuario.Token);
                    if (enviado)
                    {
                        ViewBag.Restablecido = true;
                        ViewBag.MensajeRestablecido = "Se ha enviado un correo electronico de restablecimiento";
                    }
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias con el correo";
            }
            return RedirectToAction("Login", "Login");
        }

    }
}
