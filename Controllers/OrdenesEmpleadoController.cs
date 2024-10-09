using Microsoft.AspNetCore.Mvc;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesEmpleadoController : Controller
    {

        //MUESTRA LA VISTA DE INDEX
        public IActionResult Index()
        {
            return View();
        }


        //MUESTRA LA VISTA DE CREAR ORDENES
        public IActionResult Crear()
        {
            return View();
        }


        //MUESTRA LA VISTA DE REPORTES
        public IActionResult Reportes()
        {
            return View();
        }
    }
}
