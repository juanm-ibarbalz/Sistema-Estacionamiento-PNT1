using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class ConfiguracionViewModel
{
    [Range(0, int.MaxValue, ErrorMessage = "La tarifa por hora debe ser un valor positivo.")]
    public decimal PrecioActual { get; set; }
    public Dimension Dimensiones { get; set; } = new Dimension();
}
