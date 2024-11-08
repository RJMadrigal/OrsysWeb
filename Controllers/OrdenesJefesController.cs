using Microsoft.AspNetCore.Authorization;
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




        [Authorize(Roles = "Jefe")]
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
        [Authorize(Roles = "Jefe")]
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

            // Llama a EditarOrdenJefe y recibe el éxito y el id del jefe financiero
            var (enviado, idJefeFinanciero) = await repositorioOrdenes.EditarOrdenJefe(idOrden, estado, comentarios, idUsuario);

            // Verifica si la orden fue procesada correctamente
            if (!enviado)
            {
                return View("Revisar"); // Retorna la vista actual si falla la actualización
            }

            // Intenta obtener los datos del usuario comprador de la orden
            var usuarioComprador = await servicioUsuario.ObtenerDatosUserOrden(idOrden);
            if (usuarioComprador == null)
            {
                return NotFound(); // Retorna NotFound si el comprador no existe
            }

            // Envia notificación de estado al usuario comprador
            await emailService.EnviarNotificacionEstadoJefe(usuarioComprador.Correo, usuarioComprador.Nombre, idOrden);

            // Si la orden está aprobada y hay un jefe financiero asignado, envía notificación
            if (estado == "Aprobada" && idJefeFinanciero != 0)
            {
               var jefeFinanciero = await servicioUsuario.ObtenerDatosUserPorID(idJefeFinanciero);

                await emailService.EnviarNotificacionJefeFinanciero(jefeFinanciero.Correo, jefeFinanciero.Nombre, idOrden);
               
            }

            return RedirectToAction("Index");
        }




        //NO TIENE FUNCIONALIDAD POR EL MOMENTO
        [Authorize(Roles = "Jefe")]
        public async Task<IActionResult> Reportes()
        {
            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            //OBTIENE LA LISTA DE ORDENES DEL USUARIO JEFE
            var listaOrdenes = await repositorioOrdenes.ObtenerTodasOrdenesJefes(usuarioId);

            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }




        [Authorize(Roles = "Jefe")]
        public async Task<IActionResult> VerOrdenEspecifica(int id)
        {
            //SE OBTIENE EL ID DEL USUARIO
            int idUsuario = servicioUsuario.ObtenerUsuarioId();

            if (idUsuario == null)
            {
                return NotFound();
            }

            //SE OBTIENE EL NOMBRE DEL USUARIO
            var NombreJefe = await servicioUsuario.ObtenerNombreUsuario(idUsuario);

            if (NombreJefe == null)
            {
                return NotFound();
            }

            //OBTIENE EL NOMBRE DEL JEFE FINANCIERO DE LA ORDEN
            var nombreJefeFinanciero = await servicioUsuario.ObtenerJefeFinanciero(id);


            //SE OBTIENE LA ORDEN POR ID Y SE ENVIA EL NOMBRE DEL JEFE APROBADOR
            var orden = await repositorioOrdenes.ObtenerOrdenPorId(id, NombreJefe, nombreJefeFinanciero);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }




    }
}
