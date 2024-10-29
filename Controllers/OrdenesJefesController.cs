﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services;
using SistemaOrdenes.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesJefesController : Controller
    {

        private readonly UsuarioService servicioUsuario;
        private readonly IRepositorioOrdenes repositorioOrdenes;
        private readonly IEmailService emailService;

        public OrdenesJefesController(UsuarioService servicioUsuario, IRepositorioOrdenes repositorioOrdenes, IEmailService emailService) {

            this.servicioUsuario = servicioUsuario;
            this.repositorioOrdenes = repositorioOrdenes;
            this.emailService = emailService;   

        }




        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> Index()
        {

            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            //SE OBTIENE LA LISTA DE ORDENES
            var listaOrdenes = await repositorioOrdenes.ObtenerOrdenesJefe(usuarioId);


            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }



       
        //REVISA UNA ORDEN EN ESPECIFICA
        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> Revisar(int id)
        {
            int usuarioid = servicioUsuario.ObtenerUsuarioId();

            var modelo = await repositorioOrdenes.ObtenerOrdenPorId(id);


            return View(modelo);
        }



        //ENVIA EL ESTADO DE LA ORDEN Y LOS COMENTARIOS
        [HttpPost]
        public async Task<IActionResult> ActualizarOrden(int idOrden, string estado, string comentarios)
        {
            var idUsuario = servicioUsuario.ObtenerUsuarioId();

            var enviado = await repositorioOrdenes.EditarOrdenJefe(idOrden, estado, comentarios, idUsuario);

            //SI NO SE ENVIA
            if (!enviado)
            {
                return View();
            }

            var usuarioComprador = await servicioUsuario.ObtenerDatosUserOrden(idOrden);
            // Enviar email de notificacion de estado
            if(usuarioComprador == null)
            {
                return NotFound();
            }


            var email = await emailService.EnviarNotificacionEstadoJefe(usuarioComprador.Correo, usuarioComprador.Nombre, idOrden);

            return RedirectToAction("Index");
        }





        //NO TIENE FUNCIONALIDAD POR EL MOMENTO
        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public IActionResult Reportes()
        {
            return View();
        }
    }
}
