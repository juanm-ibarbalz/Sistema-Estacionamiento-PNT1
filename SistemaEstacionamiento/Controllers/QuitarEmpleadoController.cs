using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System.Threading.Tasks;

namespace SistemaEstacionamiento.Controllers
{
    public class QuitarEmpleadoController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public QuitarEmpleadoController(DbestacionamientoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult QuitarEmpleado()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarEmpleado(decimal cuil)
        {
            var empleado = await _context.Empleados.FindAsync(cuil);
            if (empleado == null)
            {
                ModelState.AddModelError(string.Empty, "El empleado no existe.");
                return View();
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
