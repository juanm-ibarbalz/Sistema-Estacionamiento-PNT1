using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SistemaEstacionamiento.Models;

public partial class Vehiculo
{
    [Key]
    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [StringLength(7, ErrorMessage = "La matrícula no puede tener más de 7 caracteres.")]
    public string Matricula { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
    public string Tipo { get; set; } = null!;

    [StringLength(20, ErrorMessage = "El color no puede tener mas de 20 caracteres")]
    public string? Color { get; set; }

    [Required(ErrorMessage = "El piso es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El piso debe ser un valor positivo.")]
    public byte Piso { get; set; }

    [Required(ErrorMessage = "El lugar es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El lugar debe ser un valor positivo.")]
    public byte Lugar { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
