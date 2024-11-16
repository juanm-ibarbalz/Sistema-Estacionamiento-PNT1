using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class Registro
{
    [Key]
    [Required]
    public decimal CodigoRegistro { get; set; }

    [Required]
    public DateOnly FechaEntrada { get; set; }

    [Required]
    public TimeOnly HoraEntrada { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public TimeOnly? HoraSalida { get; set; }

    [Required(ErrorMessage = "La matricula es obligatoria.")]
    [StringLength(7, ErrorMessage = "La matrícula no puede tener más de 7 caracteres.")]
    public string Matricula { get; set; } = null!;

    public decimal? Importe {  get; set; }

    //public virtual Vehiculo MatriculaNavigation { get; set; } = null!;
}
