using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services;
using SistemaOrdenes.Services.Interfaces;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesEmpleadoController : Controller
    {

        private readonly UsuarioService servicioUsuario;
        private readonly IRepositorioOrdenes repositorioOrdenes;
        private readonly IEmailService emailService;
        public OrdenesEmpleadoController(UsuarioService servicioUsuario, IRepositorioOrdenes repositorioOrdenes,IEmailService emailService) 
        {
            this.emailService = emailService;
            this.servicioUsuario = servicioUsuario;
            this.repositorioOrdenes = repositorioOrdenes;
        }



        //MUESTRA LA VISTA DE INDEX
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            var listaOrdenes = await repositorioOrdenes.ObtenerOrdenesComprador(usuarioId);

            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }


        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> VerOrdenEspecifica(int id)
        {
            //SE OBTIENE EL ID DEL JEFE
            int idJefe = await servicioUsuario.ObtenerIdJefe();

            if(idJefe == null)
            {
                return NotFound();
            }

            //SE OBTIENE EL NOMBRE DEL JEFE
            var NombreJefe = await servicioUsuario.ObtenerNombreUsuario(idJefe);

            if (NombreJefe == null)
            {
                return NotFound();
            }

            var nombreJefeFinanciero = await servicioUsuario.ObtenerJefeFinanciero(id);


            //SE OBTIENE LA ORDEN POR ID Y SE ENVIA EL NOMBRE DEL JEFE APROBADOR
            var orden = await repositorioOrdenes.ObtenerOrdenPorId(id, NombreJefe, nombreJefeFinanciero);

            if(orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }


        //MUESTRA LA VISTA DE CREAR ORDENES
        [Authorize(Roles = "Empleado")]
        public IActionResult Crear()
        {
            return View();
        }


        // POST DE CREAR ORDEN
        [HttpPost]
        public async Task<IActionResult> Crear(CrearOrdenViewModel crearOrdenViewModel)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            // SE PASA EL USUARIOID AL MODELO
            crearOrdenViewModel.idUsuario = usuarioId;

            // SI EL MODELO NO ES VÁLIDO...
            if (!ModelState.IsValid)
            {
                return View(crearOrdenViewModel);
            }

            // SE CREA LA ORDEN MEDIANTE EL PROCEDIMIENTO ALMACENADO
            var idOrden = await repositorioOrdenes.CrearOrden(crearOrdenViewModel);

            // SI HUBO ALGÚN ERROR AL EJECUTAR EL PROCEDIMIENTO ALMACENADO 
            if (idOrden == null)
            {
                ModelState.AddModelError("", "Error al crear la orden");
                return View(crearOrdenViewModel);
            }

            // OBTIENE LOS DATOS DEL JEFE
            var jefe = await servicioUsuario.ObtenerDatosJefe();

            // LÓGICA PARA ENVIAR EL CORREO AL JEFE
            var envio = await emailService.EnviarJefeDirectoEmail(jefe.Correo, jefe.Nombre, idOrden.Value);

            // SI EL CORREO SE ENVÍA CON ÉXITO, REDIRIGE A LA PÁGINA PRINCIPAL
            if (envio)
            {
                return RedirectToAction("Index");
            }

            // SI HAY ERROR EN EL ENVÍO DEL CORREO, MUESTRA MENSAJE PERO PERMITE PROGRESO
            ModelState.AddModelError("", "Se ha creado la orden, pero ha ocurrido un error al notificar por email al jefe correspondiente.");
            return View(crearOrdenViewModel);
        }


        //MUESTRA LA VISTA DE REPORTES
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Reportes()
        {
            //SE OBTIENE EL ID DEL USUARIO LOGEADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            var listaOrdenes = await repositorioOrdenes.ObtenerOrdenesComprador(usuarioId);

            //ENVIA LA LISTA DE ORDENES
            return View(listaOrdenes);
        }
    }
}
