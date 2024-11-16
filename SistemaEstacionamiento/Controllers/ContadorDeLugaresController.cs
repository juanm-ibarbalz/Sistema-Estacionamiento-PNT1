using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System.Linq;

namespace SistemaEstacionamiento.Controllers
{
    public class ContadorDeLugaresController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public ContadorDeLugaresController(DbestacionamientoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetLugaresDisponibles()
        {
            var dimensiones = await _context.Dimensiones.OrderByDescending(d => d.FechaActualizacion).FirstOrDefaultAsync();
            if (dimensiones == null)
            {
                return Json(new { error = "No se han encontrado las dimensiones del estacionamiento." });
            }

            int totalEspaciosAutos = dimensiones.Pisos * dimensiones.EspaciosAuto;
            int totalEspaciosMotos = dimensiones.Pisos * dimensiones.EspaciosMoto;

            int espaciosOcupadosAutos = await _context.Vehiculos.CountAsync(v => v.Tipo == "Auto");
            int espaciosOcupadosMotos = await _context.Vehiculos.CountAsync(v => v.Tipo == "Moto");

            int espaciosDisponiblesAutos = totalEspaciosAutos - espaciosOcupadosAutos;
            int espaciosDisponiblesMotos = totalEspaciosMotos - espaciosOcupadosMotos;

            return Json(new { espaciosDisponiblesAutos, espaciosDisponiblesMotos });
        }
    }
}
