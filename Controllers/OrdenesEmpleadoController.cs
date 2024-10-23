using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrdenes.Models;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesEmpleadoController : Controller
    {

        //MUESTRA LA VISTA DE INDEX
        [Authorize(Roles = "Empleado")]
        public IActionResult Index()
        {
            return View();
        }


        //MUESTRA LA VISTA DE CREAR ORDENES
        [Authorize(Roles = "Empleado")]
        public IActionResult Crear()
        {
            return View();
        }


        //POST DE CREAR ORDEN
        [HttpPost]
        public IActionResult Crear(CrearOrdenViewModel crearOrdenViewModel)
        {
            return View();
        }


        //MUESTRA LA VISTA DE REPORTES
        [Authorize(Roles = "Empleado")]
        public IActionResult Reportes()
        {
            return View();
        }
    }
}
