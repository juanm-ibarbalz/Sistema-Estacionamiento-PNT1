using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEstacionamiento.Controllers
{
    public class RetirarVehiculoController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public RetirarVehiculoController(DbestacionamientoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RetirarVehiculo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RetirarVehiculo(RetirarVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Include = JOIN
            var ticket = await _context.Tickets
                .Include(t => t.DniNavigation) // Cliente relacionado
                .Include(t => t.MatriculaNavigation) // Vehículo relacionado
                .FirstOrDefaultAsync(t => t.CodigoTicket == model.CodigoTicket);

            if (ticket == null)
            {
                ModelState.AddModelError("", "El ticket no existe.");
                return View(model);
            }

            if (model.Dni != ticket.Dni)
            {
                ModelState.AddModelError("", "El DNI ingresado no coincide con el registrado en el ticket.");
                return View(model);
            }

            var registro = await _context.Registros
                .FirstOrDefaultAsync(r => r.Matricula == ticket.Matricula && r.FechaSalida == null);

            if (registro == null)
            {
                ModelState.AddModelError("", "No se encontró un registro activo para este vehículo.");
                return View(model);
            }

            var fechaHoraSalida = DateTime.Now;
            var fechaHoraEntrada = ticket.FechaEntrada.ToDateTime(ticket.HoraEntrada);

            registro.FechaSalida = DateOnly.FromDateTime(fechaHoraSalida);
            registro.HoraSalida = TimeOnly.FromDateTime(fechaHoraSalida);

            var horasEstacionado = (decimal)Math.Ceiling((fechaHoraSalida - fechaHoraEntrada).TotalHours);
            var importe = horasEstacionado * await ObtenerPrecioPorHora();
            
            registro.Importe = importe;

            _context.Tickets.Remove(ticket);
            _context.Vehiculos.Remove(ticket.MatriculaNavigation);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<decimal> ObtenerPrecioPorHora()
        {
            var ultimaTarifa = await _context.Tarifas
                .OrderByDescending(t => t.FechaActualizacion)
                .FirstOrDefaultAsync();

            return ultimaTarifa?.Hora ?? 0;
        }
    }
}
