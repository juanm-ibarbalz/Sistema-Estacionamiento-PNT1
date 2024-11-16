using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class ConfiguracionViewModel
{
    [Range(0, int.MaxValue, ErrorMessage = "La tarifa por hora debe ser un valor positivo.")]
    public decimal PrecioActual { get; set; }
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Dimension? Dimensiones { get; set; }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
