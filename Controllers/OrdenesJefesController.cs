using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaOrdenes.Controllers
{
    public class OrdenesJefesController : Controller
    {
        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public IActionResult Reportes()
        {
            return View();
        }

        [Authorize(Roles = "Jefe, Jefe aprobador 1, Jefe aprobador 2, Jefe aprobador 3")]
        public IActionResult Revisar()
        {
            return View();
        }
    }
}
