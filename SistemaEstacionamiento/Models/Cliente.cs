using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class Cliente
{
    [Key]
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [Range(0, 99999999, ErrorMessage = "El dni puede contener hasta 8 caracteres.")]
    public decimal Dni { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(20, ErrorMessage = "El nombre no puede tener más de 20 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(20, ErrorMessage = "El apellido no puede tener más de 20 caracteres.")]
    public string Apellido { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
