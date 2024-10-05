﻿using System;
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


        //GET AL ENTRAR AL FORM PARA CREAR UN USUARIO
        [HttpGet]
        public IActionResult Create()
        {
            //CARGA LA LISTA DE ROLES Y JEFES
            CargarListasDeSeleccion();
            return View();
        }


        //POST PARA CREAR EL USUARIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Usuario,Correo,IdRol,IdJefe")] TbUsuario usuarios, [FromServices] IWebHostEnvironment env)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarios);
               
            }

            //SE GENERA UNA CLAVE
            usuarios.Clave = GenerateToken.GenerateTempPass();

            //SE GENERA UN TOKEN
            usuarios.Token = GenerateToken.Generate();

            //SE ENVIA EL MODELO MEDIANTE EL SERVICIO QUE REGISTRA EL USUARIO
            await _usuarioService.RegistrarUsuario(usuarios);

            //SE ENVÍA UN EMAIL AL USUARIO, ENVIANDO EL CORREO, EL NOMBRE Y EL TOKEN GENERADO
            var emailResult = await _emailService.SendConfirmationEmail(usuarios.Correo, usuarios.Nombre, usuarios.Token);

            //SE REDIRIGE AL INDEX
            return RedirectToAction("Index");
            

        }



        //GET AL INGRESAR A EDITAR EL USUARIO
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //BUSCA EL USUARIO POR ID
            var usuarios = await _context.TbUsuarios.FindAsync(id);

            //SI NO EXISTE EL USUARIO, SE MUESTRA UN MENSAJE
            if (usuarios == null)
            {
                return NotFound();
            }

            //SE MAPEA LOS DATOS AL MODELO
            var viewModel = new EditarUsuarioViewModel
            {
                IdUsuario = usuarios.IdUsuario,
                Nombre = usuarios.Nombre,
                Usuario = usuarios.Usuario,
                Correo = usuarios.Correo,
                Restablecer = usuarios.Restablecer,
                Confirmado = usuarios.Confirmado,
                IdRol = usuarios.IdRol,
                IdJefe = usuarios.IdJefe,
            };

            //CARGA LOS SELECT ITEM DE ROLES Y USUARIOS JEFES
            CargarListasDeSeleccion(viewModel.IdJefe, viewModel.IdRol);
            return View(viewModel);
        }



        //POST PARA EDITAR EL USUARIO
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(EditarUsuarioViewModel modelo) //[Bind("IdUsuario,Nombre,Usuario,Correo,Restablecer,Confirmado,IdRol,IdJefe")] )
        {

            //SE VALIDA EL MODELO
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                //SE ENVIA EL MODELO PARA EDITAR EL USUARIO
                await repositorioUsuarios.EditarUser(modelo);
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UsuariosExists(modelo.IdUsuario))
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



        //GET AL SELECCIONAR EL BOTON DE ELIMINAR USUARIO, EL CUAL CARGA LOS DATOS DEL USUARIO A ELIMINAR
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //SE VALIDA EL ID
            if (id == null)
            {
                return NotFound();
            }

            //SE OBTIENE EL USUARIO MEDIANTE EL ID
            var usuarios = await _context.TbUsuarios
                .Include(u => u.IdJefeNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            //SE VALIDA SI EL USUARIO EXISTE...
            if (usuarios == null)
            {
                return NotFound();
            }

            //DEVUELVE LA VISTA CON LOS DATOS
            return View(usuarios);
        }




        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            //SE OBTIENE EL USUARIO A ELIMINAR MEDIANTE EL ID
            var usuarios = await _context.TbUsuarios.FindAsync(id);


            //SE VALIDA SI EL USUARIO EXISTE...
            if (usuarios == null)
            {
                return NotFound();
            }

            //SE ELIMINA EL USUARIO UTILIZANDO EF Y SE ENVIA EL MODELO
            _context.TbUsuarios.Remove(usuarios);

            //SE GUARDA LOS CAMBIOS
            await _context.SaveChangesAsync();

            //SE REDIRIGE AL INDEX
            return RedirectToAction("Index");
        }

 

        //METODO PARA CARGAR EN LISTAS LOS USUARIOS JEFES Y LOS ROLES
        private void CargarListasDeSeleccion(int? idJefe = null, int? idRol = null)
        {
            ViewBag.Jefes = new SelectList(_usuarioService.ObtenerJefes(), "IdUsuario", "Nombre", idJefe);
            ViewBag.Roles = new SelectList(_usuarioService.ObtenerRoles(), "IdRol", "NombreRol", idRol);
        }


        //METODO QUE VALIDA SI EL USUARIO EXISTE
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
