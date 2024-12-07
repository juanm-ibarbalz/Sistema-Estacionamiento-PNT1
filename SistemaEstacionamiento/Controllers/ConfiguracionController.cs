using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
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
        var precioActual = await _context.Tarifas
            .OrderByDescending(t => t.FechaActualizacion)
            .Select(t => t.Hora)
            .FirstOrDefaultAsync();

        var ultimaDimension = await _context.Dimensiones
            .OrderByDescending(d => d.FechaActualizacion)
            .FirstOrDefaultAsync();

        var model = new ConfiguracionViewModel
        {
            PrecioActual = precioActual,
            Dimensiones = ultimaDimension ?? new Dimension()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PrecioPorHora(ConfiguracionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Configuracion", model);
        }

        var nuevoCodigoTarifa = await ObtenerSiguienteCodigoAsync<Tarifa>(t => t.CodigoTarifa);

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

        var nuevoCodigoDimension = await ObtenerSiguienteCodigoAsync<Dimension>(d => d.CodigoDimension);

        var dimension = new Dimension
        {
            CodigoDimension = nuevoCodigoDimension,
            Pisos = model.Dimensiones.Pisos,
            EspaciosAuto = model.Dimensiones.EspaciosAuto,
            EspaciosMoto = model.Dimensiones.EspaciosMoto,
            FechaActualizacion = DateTime.Now
        };

        _context.Dimensiones.Add(dimension);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Configuracion));
    }

    //Una lamba de como obtener el codigo
    private async Task<decimal> ObtenerSiguienteCodigoAsync<T>(Expression<Func<T, decimal>> codigo) where T : class
    {
        var ultimoCodigo = await _context.Set<T>()
            .OrderByDescending(codigo)
            .Select(codigo)
            .FirstOrDefaultAsync();

        return ultimoCodigo + 1;
    }
}


