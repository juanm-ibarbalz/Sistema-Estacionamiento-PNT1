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
            if (!ModelState.IsValid)
                return View(model);

            var ticket = await _context.Tickets
                .Where(t => t.CodigoTicket == model.CodigoTicket)
                .Select(t => new
                {
                    Ticket = t,
                    Vehiculo = _context.Vehiculos.FirstOrDefault(v => v.Matricula == t.Matricula),
                    Registro = _context.Registros.FirstOrDefault(r => r.Matricula == t.Matricula && r.FechaSalida == null)
                })
                .FirstOrDefaultAsync();

            if (ticket == null || ticket.Vehiculo == null)
            {
                ModelState.AddModelError("", "El ticket o el vehículo asociado no existen.");
                return View(model);
            }

            if (model.Dni != ticket.Ticket.Dni)
            {
                ModelState.AddModelError("", "El DNI ingresado no coincide con el registrado en el ticket.");
                return View(model);
            }

            var fechaHoraSalida = DateTime.Now;
            var fechaHoraEntrada = ticket.Ticket.FechaEntrada.ToDateTime(ticket.Ticket.HoraEntrada);
            var horasEstacionado = Math.Ceiling((fechaHoraSalida - fechaHoraEntrada).TotalHours);
            var precioPorHora = await ObtenerPrecioPorHoraDesdeBaseDeDatosAsync();
            var importe = Math.Ceiling((decimal)horasEstacionado) * precioPorHora;

            if (ticket.Registro == null)
            {
                ModelState.AddModelError("", "No se ha encontrado un registro de los datos ingresados.");
                return View(model);
            }

            ticket.Registro.FechaSalida = DateOnly.FromDateTime(fechaHoraSalida);
            ticket.Registro.HoraSalida = TimeOnly.FromDateTime(fechaHoraSalida);
            ticket.Registro.Importe = importe;

            _context.Tickets.Remove(ticket.Ticket);
            _context.Vehiculos.Remove(ticket.Vehiculo);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<decimal> ObtenerPrecioPorHoraDesdeBaseDeDatosAsync()
        {
            var ultimaTarifa = await _context.Tarifas
                .OrderByDescending(t => t.FechaActualizacion)
                .FirstOrDefaultAsync();

            return ultimaTarifa?.Hora ?? 0;
        }
    }
}
