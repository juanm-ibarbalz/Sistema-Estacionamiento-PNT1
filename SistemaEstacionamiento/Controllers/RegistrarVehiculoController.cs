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

            var dimensiones = await ObtenerUltimasDimensionesAsync();
            if (dimensiones == null)
            {
                ModelState.AddModelError("", "Error al obtener las dimensiones del estacionamiento.");
                return View(model);
            }

            if (model.Piso.HasValue && model.Lugar.HasValue)
            {
                if (!ValidarRangoLugar(model.Piso.Value, model.Lugar.Value, model.Tipo, dimensiones))
                {
                    ModelState.AddModelError("", "El lugar ingresado está fuera de rango.");
                    return View(model);
                }

                if (!await VerificarDisponibilidadLugarAsync(model.Piso.Value, model.Lugar.Value))
                {
                    ModelState.AddModelError("", "El lugar ingresado no está disponible.");
                    return View(model);
                }
            }
            else
            {
                var lugarAsignado = await ObtenerProximoLugarDisponibleAsync(model.Tipo, dimensiones);
                if (lugarAsignado == null)
                {
                    ModelState.AddModelError("", "No hay lugares disponibles.");
                    return View(model);
                }

                model.Piso = lugarAsignado.Value.Piso;
                model.Lugar = lugarAsignado.Value.Lugar;
            }

            if (await VehiculoYaRegistradoAsync(model.Matricula))
            {
                ModelState.AddModelError("", "El vehículo ya está registrado.");
                return View(model);
            }

            if (!await ClienteExisteAsync(model.Dni))
            {
                RegistrarNuevoCliente(model);
            }

            RegistrarNuevoVehiculo(model);
            await RegistrarNuevoTicketAsync(model);
            await RegistrarNuevoRegistroAsync(model);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<Dimension?> ObtenerUltimasDimensionesAsync()
        {
            return await _context.Dimensiones
                .OrderByDescending(d => d.FechaActualizacion)
                .FirstOrDefaultAsync();
        }

        private bool ValidarRangoLugar(byte piso, byte lugar, string tipoVehiculo, Dimension dimensiones)
        {
            if (piso > dimensiones.Pisos) return false;
            var maxLugares = tipoVehiculo == "Auto" ? dimensiones.EspaciosAuto : dimensiones.EspaciosMoto;
            return lugar <= maxLugares;
        }

        private async Task<bool> VerificarDisponibilidadLugarAsync(byte piso, byte lugar)
        {
            return !await _context.Vehiculos.AnyAsync(v => v.Piso == piso && v.Lugar == lugar);
        }

        private async Task<(byte Piso, byte Lugar)?> ObtenerProximoLugarDisponibleAsync(string tipoVehiculo, Dimension dimensiones)
        {
            for (byte piso = 1; piso <= dimensiones.Pisos; piso++)
            {
                if (tipoVehiculo == "Auto")
                {
                    var totalAutos = await _context.Vehiculos.CountAsync(v => v.Piso == piso && v.Tipo == "Auto");
                    if (totalAutos < dimensiones.EspaciosAuto)
                    {
                        return (piso, (byte)(totalAutos + 1));
                    }
                }
                else if (tipoVehiculo == "Moto")
                {
                    var totalMotos = await _context.Vehiculos.CountAsync(v => v.Piso == piso && v.Tipo == "Moto");
                    if (totalMotos < dimensiones.EspaciosMoto)
                    {
                        // Las motos comienzan después de los espacios para autos
                        return (piso, (byte)(dimensiones.EspaciosAuto + totalMotos + 1));
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
                Tipo = model.Tipo,
                Color = model.Color?.ToUpper(),
                Piso = model.Piso.Value,
                Lugar = model.Lugar.Value
            };
            _context.Vehiculos.Add(nuevoVehiculo);
        }

        private async Task RegistrarNuevoTicketAsync(RegistrarVehiculoViewModel model)
        {
            var nuevoTicket = new Ticket
            {
                CodigoTicket = await GenerarCodigoUnicoAsync<Ticket>(t => t.CodigoTicket),
                FechaEntrada = DateOnly.FromDateTime(DateTime.Now),
                HoraEntrada = TimeOnly.FromDateTime(DateTime.Now),
                Matricula = model.Matricula.ToUpper(),
                Dni = model.Dni
            };
            _context.Tickets.Add(nuevoTicket);
        }

        private async Task RegistrarNuevoRegistroAsync(RegistrarVehiculoViewModel model)
        {
            var nuevoRegistro = new Registro
            {
                CodigoRegistro = await GenerarCodigoUnicoAsync<Registro>(r => r.CodigoRegistro),
                FechaEntrada = DateOnly.FromDateTime(DateTime.Now),
                HoraEntrada = TimeOnly.FromDateTime(DateTime.Now),
                Matricula = model.Matricula.ToUpper()
            };
            _context.Registros.Add(nuevoRegistro);
        }

        private async Task<decimal> GenerarCodigoUnicoAsync<T>(Func<T, decimal> selector) where T : class
        {
            var maxCodigo = await _context.Set<T>().MaxAsync(t => (decimal?)selector(t)) ?? 0;
            return maxCodigo + 1;
        }
    }
}
