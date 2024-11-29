using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Models;
using System.Linq;

namespace SistemaEstacionamiento.Controllers
{
    public class GananciasEstimadasController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public GananciasEstimadasController(DbestacionamientoContext context)
        {
            _context = context;
        }

        public IActionResult GananciasEstimadas()
        {
            #pragma warning disable CS8629 // Nullable value type may be null.
            var gananciasRegistradas = _context.Registros
                .Where(r => r.Importe.HasValue)
                .Sum(r => r.Importe.Value);
            #pragma warning restore CS8629 // Nullable value type may be null.

            var estimacionGanancias = CalcularGananciasEstimadas();
            var sueldosTotales = _context.Empleados.Sum(e => e.Sueldo);
            var gananciasTotales = (gananciasRegistradas + estimacionGanancias) - sueldosTotales;

            var viewModel = new GananciasEstimadasViewModel
            {
                GananciasRegistradas = gananciasRegistradas,
                GananciasEstimadas = estimacionGanancias,
                SueldosTotales = sueldosTotales,
                GananciasTotales = gananciasTotales
            };

            return View(viewModel);
        }

        private decimal CalcularGananciasEstimadas()
        {
            var tarifa = _context.Tarifas.FirstOrDefault()?.Hora ?? 0;
            if (tarifa == 0)
                return 0;

            var vehiculosEstacionados = _context.Vehiculos.Count();
            return vehiculosEstacionados * tarifa;
        }
    }
}
