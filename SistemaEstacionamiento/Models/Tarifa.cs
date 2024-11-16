using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class Tarifa
{
    [Key]
    [Required]
    public decimal CodigoTarifa { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria.")]
    public decimal Hora { get; set; }

    [Required]
    public DateTime FechaActualizacion { get; set; }
}
