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
            var mesActual = DateTime.Now.Month;
            var anioActual = DateTime.Now.Year;

            var gananciasRegistradas = _context.Registros
                .Where(r => r.FechaEntrada.Month == mesActual && r.FechaEntrada.Year == anioActual && r.Importe.HasValue)
                .Sum(r => r.Importe.Value);

            var estimacionGanancias = CalcularGananciasEstimadas();
            var sueldosTotales = _context.Empleados.Sum(e => e.Sueldo);
            var gananciasTotales = (gananciasRegistradas + estimacionGanancias) - sueldosTotales;

            var viewModel = new GananciasEstimadasViewModel
            {
                GananciasRegistradas = gananciasRegistradas,
                GananciasEstimadas = estimacionGanancias,
                SueldosTotales = sueldosTotales,
                GananciasTotales = gananciasTotales,
                Mes = mesActual,
                Anio = anioActual
            };

            return View(viewModel);
        }


        private decimal CalcularGananciasEstimadas()
        {
            var tarifa = _context.Tarifas
                .OrderByDescending(t => t.FechaActualizacion)
                .FirstOrDefault()?.Hora ?? 0;
            if (tarifa == 0) return 0;

            var gananciasEstimadas = _context.Registros
                .Where(r => r.FechaSalida == null) 
                .AsEnumerable()
                .Sum(r =>
                {
                    var fechaEntrada = r.FechaEntrada.ToDateTime(r.HoraEntrada);
                    var horasEstacionadas = Math.Ceiling((DateTime.Now - fechaEntrada).TotalHours);
                    return (decimal)horasEstacionadas * tarifa;
                });

            return gananciasEstimadas;
        }
    }
}
