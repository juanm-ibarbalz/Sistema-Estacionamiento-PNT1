using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Controllers
{
    public class RegistrarVehiculoController : Controller
    {
        private readonly DbestacionamientoContext _context;

        public RegistrarVehiculoController(DbestacionamientoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RegistrarVehiculo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVehiculo(RegistrarVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await VehiculoYaRegistradoAsync(model.Matricula))
            {
                ModelState.AddModelError(string.Empty, "El vehículo ya está registrado.");
                return View(model);
            }

            var dimensiones = await ObtenerUltimasDimensionesAsync();
            if (dimensiones == null)
            {
                ModelState.AddModelError(string.Empty, "Error al obtener las dimensiones del estacionamiento.");
                return View(model);
            }

            if (model.Piso.HasValue && model.Lugar.HasValue)
            {
                if (!ValidarRangoLugar(model.Piso.Value, model.Lugar.Value, model.Tipo, dimensiones))
                {
                    ModelState.AddModelError(string.Empty, "El lugar ingresado está fuera de rango.");
                    return View(model);
                }

                if (!await VerificarDisponibilidadLugarAsync(model.Piso.Value, model.Lugar.Value))
                {
                    ModelState.AddModelError(string.Empty, "El lugar ingresado no está disponible.");
                    return View(model);
                }
            }
            else
            {
                var lugarAsignado = await ObtenerProximoLugarDisponibleAsync(model.Tipo, dimensiones);
                if (lugarAsignado == null)
                {
                    ModelState.AddModelError(string.Empty, "No hay lugares disponibles.");
                    return View(model);
                }

                model.Piso = lugarAsignado.Value.Piso;
                model.Lugar = lugarAsignado.Value.Lugar;
            }

            if (!await ClienteExisteAsync(model.Dni))
            {
                RegistrarNuevoCliente(model);
            }

            RegistrarNuevoVehiculo(model);
            RegistrarNuevoTicketAsync(model);
            RegistrarNuevoRegistroAsync(model);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<Dimension?> ObtenerUltimasDimensionesAsync()
        {
            return await _context.Dimensiones
                .OrderByDescending(d => d.FechaActualizacion)
                .FirstOrDefaultAsync();
        }

        private bool ValidarRangoLugar(int piso, int lugar, string tipoVehiculo, Dimension dimensiones)
        {
            if (piso > dimensiones.Pisos) return false;
            var maxLugares = tipoVehiculo == "Auto" ? dimensiones.EspaciosAuto : dimensiones.EspaciosMoto;
            return lugar <= maxLugares;
        }

        private async Task<bool> VerificarDisponibilidadLugarAsync(int piso, int lugar)
        {
            return !await _context.Vehiculos.AnyAsync(v => v.Piso == piso && v.Lugar == lugar);
        }

        private async Task<(int Piso, int Lugar)?> ObtenerProximoLugarDisponibleAsync(string tipoVehiculo, Dimension dimensiones)
        {
            for (int piso = 1; piso <= dimensiones.Pisos; piso++)
            {
                var maxLugares = tipoVehiculo == "Auto" ? dimensiones.EspaciosAuto : dimensiones.EspaciosMoto;

                var lugaresOcupados = await _context.Vehiculos
                    .Where(v => v.Piso == piso && v.Tipo == tipoVehiculo)
                    .Select(v => v.Lugar)
                    .ToListAsync();

                for (int lugar = (tipoVehiculo == "Auto" ? 1 : dimensiones.EspaciosAuto + 1);
                         lugar <= (tipoVehiculo == "Auto" ? dimensiones.EspaciosAuto : dimensiones.EspaciosAuto + maxLugares);
                         lugar++)
                {
                    if (!lugaresOcupados.Contains(lugar))
                    {
                        return (piso, lugar);
                    }
                }
            }

            return null;
        }



        private async Task<bool> VehiculoYaRegistradoAsync(string matricula)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Matricula == matricula.ToUpper());
        }

        private async Task<bool> ClienteExisteAsync(decimal dni)
        {
            return await _context.Clientes.AnyAsync(c => c.Dni == dni);
        }

        private void RegistrarNuevoCliente(RegistrarVehiculoViewModel model)
        {
            var nuevoCliente = new Cliente
            {
                Dni = model.Dni,
                Nombre = model.Nombre,
                Apellido = model.Apellido
            };
            _context.Clientes.Add(nuevoCliente);
        }

        private void RegistrarNuevoVehiculo(RegistrarVehiculoViewModel model)
        {
            var nuevoVehiculo = new Vehiculo
            {
                Matricula = model.Matricula.ToUpper(),
                Tipo = model.Tipo.ToUpper(),
                Color = model.Color?.ToUpper(),
                Piso = model.Piso.Value,
                Lugar = model.Lugar.Value
            };
            _context.Vehiculos.Add(nuevoVehiculo);
        }

        private void RegistrarNuevoTicketAsync(RegistrarVehiculoViewModel model)
        {
            var nuevoTicket = new Ticket
            {
                CodigoTicket = GenerarCodigoUnico(),
                FechaEntrada = DateOnly.FromDateTime(DateTime.Now),
                HoraEntrada = TimeOnly.FromDateTime(DateTime.Now),
                Matricula = model.Matricula.ToUpper(),
                Dni = model.Dni
            };
            _context.Tickets.Add(nuevoTicket);
        }

        private void RegistrarNuevoRegistroAsync(RegistrarVehiculoViewModel model)
        {
            var nuevoRegistro = new Registro
            {
                CodigoRegistro = GenerarCodigoUnico(),
                FechaEntrada = DateOnly.FromDateTime(DateTime.Now),
                HoraEntrada = TimeOnly.FromDateTime(DateTime.Now),
                Matricula = model.Matricula.ToUpper()
            };
            _context.Registros.Add(nuevoRegistro);
        }

        private decimal GenerarCodigoUnico()
        {
            var codigo = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return decimal.Parse(codigo);
        }
    }
}
