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

        // GET: Empleado/Create
        [HttpGet]
        public IActionResult QuitarEmpleado()
        {
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarEmpleado(decimal cuil)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Cuil == cuil);
            if (empleado == null)
            {
                return View();
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
