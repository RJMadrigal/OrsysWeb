using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services;
using SistemaOrdenes.Services.Interfaces;
using static SistemaOrdenes.Services.EmailService;



namespace SistemaOrdenes.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DbProyectoAnalisisIiContext _context;
        private readonly IRepositorioUsuarios repositorioUsuarios;

        //private readonly EmailService _emailService;    
        private readonly UsuarioService _usuarioService;
        private readonly IEmailService _emailService;

        public UsuariosController(DbProyectoAnalisisIiContext context, IRepositorioUsuarios repositorioUsuarios, IEmailService emailService, UsuarioService usuarioService)
        {
            _context = context;
            this.repositorioUsuarios = repositorioUsuarios;
            _emailService = emailService;
            _usuarioService = usuarioService;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var dbPruebaOrdenesContext = _context.TbUsuarios.Include(u => u.IdJefeNavigation).Include(u => u.IdRolNavigation);
            return View(await dbPruebaOrdenesContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.TbUsuarios
                .Include(u => u.IdJefeNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            CargarListasDeSeleccion();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Usuario,Correo,IdRol,IdJefe")] TbUsuario usuarios, [FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            { 
                try
                {
                    usuarios.Clave = GenerateToken.GenerateTempPass();
                    usuarios.Token = GenerateToken.Generate();
       
                    await _usuarioService.RegistrarUsuario(usuarios); 
                    
                    var emailResult = await _emailService.SendConfirmationEmail(usuarios.Correo,usuarios.Nombre,usuarios.Token);

                    //  ViewBag.Creado = true;
                    //  ViewBag.Mensaje = $"La cuenta ha sido creada. Hemos enviado un mensaje al correo {usuarios.Correo} para confirmar su cuenta";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al guardar en la base de datos: " + ex.Message);
                }
            }
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.TbUsuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            CargarListasDeSeleccion(usuarios.IdJefe, usuarios.IdRol);
            return View(usuarios);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarUsuarioViewModel usuario) //[Bind("IdUsuario,Nombre,Usuario,Correo,Restablecer,Confirmado,IdRol,IdJefe")] )
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            try
            {
                await repositorioUsuarios.EditarUser(id, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UsuariosExists(usuario.IdUsuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.TbUsuarios
                .Include(u => u.IdJefeNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.TbUsuarios.FindAsync(id);
            if (usuarios != null)
            {
                _context.TbUsuarios.Remove(usuarios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

 
        private void CargarListasDeSeleccion(int? idJefe = null, int? idRol = null)
        {
            ViewBag.Jefes = new SelectList(_usuarioService.ObtenerJefes(), "IdUsuario", "Nombre", idJefe);
            ViewBag.Roles = new SelectList(_usuarioService.ObtenerRoles(), "IdRol", "NombreRol", idRol);
        }
        private bool UsuariosExists(int id)
        {
            return _context.TbUsuarios.Any(e => e.IdUsuario == id);
        }






        [HttpPost]
        public async Task<ActionResult> Restablecer(string correo, [FromServices] IWebHostEnvironment env)
        {
            var usuario = await repositorioUsuarios.Obtener(correo);

            ViewBag.Correo = correo;
            if (usuario != null)
            {
                bool respuesta = await _usuarioService.RestablecerActualizarAsync(true, usuario.Clave, usuario.Token);
                if (respuesta)
                {
                    Debug.WriteLine("Correo: " + usuario.Correo);
                    bool enviado = await _emailService.SendResetPasswordEmail(correo,usuario.Nombre,usuario.Token);
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

        public async Task<ActionResult> Confirmar(string token)
        {
            var usuario = await _usuarioService.ConfirmarAsync(token);

            if (usuario != null && usuario.Confirmado == false)
            {
                return RedirectToAction("EstablecerContraseña", new { token = token });
            }
            else if(usuario.Confirmado == true)
            {
                ViewBag.Mensaje = "Su cuenta ya ha sido confirmada, ingrese con su contraseña.";
            }
            else
            {
                ViewBag.Mensaje = "Error al confirmar la cuenta.";
            }

            return View();
        }

        //no restringir
        public ActionResult EstablecerContraseña(string token)
        {
            ViewBag.Token = token;
            return View();
        }
        //no restringir
        [HttpPost]
        public async Task<ActionResult> EstablecerContraseña(string token, string clave, string confirmarClave)
        {
            if (clave != confirmarClave)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = await _usuarioService.EstablecerContraseñaActivacion(token, HashSHA256.CSHA256(clave));

            if (respuesta)
            {
                ViewBag.ContraseñaEstablecida = true;
                return View();
            }
            else
            {
                ViewBag.Mensaje = "Error al establecer la contraseña.";
                return View();
            }
        }

        //no restringir
        public async Task<ActionResult> ActualizarContraseña(string token)
        {
            var usuario = await _usuarioService.ConfirmarAsync(token);
            if (usuario.Restablecer == true)
            {
                ViewBag.Token = token;
                return View();
            }
            else
            {
                ViewBag.Restablecido = true;
                ViewBag.Mensaje = "Su contraseña ya ha sido restablecida.";
                return View();
            }
        }
        //no restringir
        [HttpPost]
        public async Task<ActionResult> ActualizarContraseña(string token, string clave, string confirmarClave)
        {
            ViewBag.Token = token;
            if (clave != confirmarClave)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = await _usuarioService.RestablecerActualizarAsync(false, Services.HashSHA256.CSHA256(clave), token);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar la contraseña";

            return View();
        }

    }
}
