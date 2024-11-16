using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class Dimension
{
    [Key]
    [Required]
    public decimal CodigoDimension { get; set; }

    [Required(ErrorMessage = "El piso es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de pisos debe ser un valor positivo.")]
    public int Pisos { get; set; }

    [Required(ErrorMessage = "La cantidad de espacios para autos es obligatoria.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de Espacios de Auto debe ser un valor positivo.")]
    public int EspaciosAuto { get; set; }

    [Required(ErrorMessage = "La cantidad de espacios para motos es obligatoria.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad de Espacios de Moto debe ser un valor positivo.")]
    public int EspaciosMoto { get; set; }

    [Required]
    public DateTime FechaActualizacion { get; set; }

}
