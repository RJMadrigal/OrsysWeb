using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrdenes.Services;
using SistemaOrdenes.Services.Interfaces;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesJefesFinancierosController : Controller
    {
        private readonly UsuarioService servicioUsuario;
        private readonly IRepositorioOrdenes repositorioOrdenes;
        private readonly IEmailService emailService;

        public OrdenesJefesFinancierosController(UsuarioService servicioUsuario, IRepositorioOrdenes repositorioOrdenes, IEmailService emailService)
        {
            this.servicioUsuario = servicioUsuario;
            this.repositorioOrdenes = repositorioOrdenes;
            this.emailService = emailService;
        }


        [Authorize(Roles = "Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> Index()
        {

            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            //SE OBTIENE LA LISTA DE ORDENES
            var listaOrdenes = await repositorioOrdenes.ObtenerOrdenesJefeFinanciero(usuarioId);


            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }




        //REPORTES DE TODAS LAS ORDENES
        [Authorize(Roles = "Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> Reportes()
        {
            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            //OBTIENE LA LISTA DE ORDENES DEL USUARIO JEFE
            var listaOrdenes = await repositorioOrdenes.ObtenerTodasOrdenesJefes(usuarioId);

            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }




        //REVISA UNA ORDEN EN ESPECIFICA
        [Authorize(Roles = "Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> Revisar(int id)
        {
            int usuarioid = servicioUsuario.ObtenerUsuarioId();

            var modelo = await repositorioOrdenes.ObtenerOrdenPorId(id);

            return View(modelo);
        }





        //VER ORDEN ESPECIFICA
        [Authorize(Roles = "Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public async Task<IActionResult> VerOrdenEspecifica(int id)
        {
            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int idUsuario = servicioUsuario.ObtenerUsuarioId();

            if (idUsuario == null)
            {
                return NotFound();
            }

            //SE OBTIENE EL NOMBRE DEL JEFE DEL USUARIO QUE APROBÓ LA ORDEN
            var NombreJefe = await servicioUsuario.ObtenerNombreUsuarioAprobador(id);

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



        [HttpPost]
        public async Task<IActionResult> ActualizarOrden(int idOrden, string estado, string comentarios)
        {
            //SE OBTIENE EL ID DEL USUARIO
            var idUsuario = servicioUsuario.ObtenerUsuarioId();

            //SE ACTUALIZA LA ORDEN
            var enviado = await repositorioOrdenes.EditarOrdenJefeFinanciero(idOrden, estado, comentarios, idUsuario);

            //SI NO SE ENVIA
            if (!enviado)
            {
                return View();
            }

            var usuarioComprador = await servicioUsuario.ObtenerDatosUserOrden(idOrden);
            // Enviar email de notificacion de estado
            if (usuarioComprador == null)
            {
                return NotFound();
            }


            var email = await emailService.EnviarNotificacionEstadoJefe(usuarioComprador.Correo, usuarioComprador.Nombre, idOrden);

            return RedirectToAction("Index");
        }
    }
}
