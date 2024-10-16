using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        //MUESTRA LA VISTA DE REPORTES
        [Authorize(Roles = "Empleado")]
        public IActionResult Reportes()
        {
            return View();
        }
    }
}
