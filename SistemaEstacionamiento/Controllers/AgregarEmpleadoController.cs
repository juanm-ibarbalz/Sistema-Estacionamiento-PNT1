using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System.Threading.Tasks;

namespace SistemaEstacionamiento.Controllers
{
    public class AgregarEmpleadoController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public AgregarEmpleadoController(DbestacionamientoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AgregarEmpleado()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarEmpleado(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(empleado);
        }
    }
}
