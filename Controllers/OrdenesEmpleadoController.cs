using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesEmpleadoController : Controller
    {

        private readonly UsuarioService servicioUsuario;
        private readonly IRepositorioOrdenes repositorioOrdenes;

        public OrdenesEmpleadoController(UsuarioService servicioUsuario, IRepositorioOrdenes repositorioOrdenes) {

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


        //MUESTRA LA VISTA DE CREAR ORDENES
        [Authorize(Roles = "Empleado")]
        public IActionResult Crear()
        {
            return View();
        }


        //POST DE CREAR ORDEN
        [HttpPost]
        public async Task<IActionResult> Crear(CrearOrdenViewModel crearOrdenViewModel)
        {
            //SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            int usuarioId = servicioUsuario.ObtenerUsuarioId();


            //SE PASA EL USUARIOID AL MODELO
            crearOrdenViewModel.idUsuario = usuarioId;


            //SI EL MODELO NO ES VALIDO...
            if(!ModelState.IsValid)
            {
                return View(crearOrdenViewModel);
            }

            //SE CREA LA ORDEN MEDIANTE EL PROCEDIMIENTO ALMACENADO
            var resultado = await repositorioOrdenes.CrearOrden(crearOrdenViewModel);


            //SI HUBO ALGUN ERROR AL EJECUTAR EL PROCEDIMIENTO ALMACENADO...
            if(resultado == false)
            {
                ModelState.AddModelError("", "Error al crear la orden");
                return View(resultado);
            }
            else
            {

                //OBTIENE EL CORREO DEL JEFE
                var obtenerCorreoJefe  = await servicioUsuario.ObtenerCorreoJefe();


                //LOGICA PARA ENVIAR EL CORREO AL JEFE




                return RedirectToAction("Index");
            }

        }


        //MUESTRA LA VISTA DE REPORTES
        [Authorize(Roles = "Empleado")]
        public IActionResult Reportes()
        {
            return View();
        }
    }
}
