using Microsoft.AspNetCore.Mvc;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesJefesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reportes()
        {
            return View();
        }

        public IActionResult Revisar()
        {
            return View();
        }
    }
}
