using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ConfiguracionController : Controller
{
    private readonly DbestacionamientoContext _context;

    public ConfiguracionController(DbestacionamientoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Configuracion()
    {
        var model = new ConfiguracionViewModel
        {
            PrecioActual = await ObtenerPrecioActualAsync(),
            Dimensiones = await _context.Dimensiones.OrderByDescending(d => d.FechaActualizacion).FirstOrDefaultAsync() ?? new Dimension()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PrecioPorHora(ConfiguracionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
            return View("Configuracion", model);
        }

        decimal ultimoCodigoTarifa = _context.Tarifas.OrderByDescending(t => t.CodigoTarifa).Select(t => t.CodigoTarifa).FirstOrDefault();
        decimal nuevoCodigoTarifa = ultimoCodigoTarifa + 1;

        var tarifa = new Tarifa
        {
            CodigoTarifa = nuevoCodigoTarifa,
            Hora = model.PrecioActual,
            FechaActualizacion = DateTime.Now
        };
        _context.Tarifas.Add(tarifa);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Configuracion));
    }

    [HttpPost]
    public async Task<IActionResult> Dimensiones(ConfiguracionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Configuracion", model);
        }

        decimal ultimoCodigoDimension = _context.Dimensiones.OrderByDescending(d => d.CodigoDimension).Select(d => d.CodigoDimension).FirstOrDefault();
        decimal nuevoCodigoDimension = ultimoCodigoDimension + 1;

        model.Dimensiones.CodigoDimension = nuevoCodigoDimension;
        model.Dimensiones.FechaActualizacion = DateTime.Now;

        _context.Dimensiones.Add(model.Dimensiones);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Configuracion));
    }

    private async Task<decimal> ObtenerPrecioActualAsync()
    {
        var ultimaTarifa = await _context.Tarifas.OrderByDescending(t => t.FechaActualizacion).FirstOrDefaultAsync();
        return ultimaTarifa != null ? ultimaTarifa.Hora : 0;
    }
}
