﻿using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models
{
    public partial class Empleado
    {
        [Key]
        [Required(ErrorMessage = "El CUIL es obligatorio.")]
        [Range(0, 99999999999, ErrorMessage = "El cuil puede contener hasta 11 caracteres")]
        public decimal Cuil { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre no puede tener más de 20 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(20, ErrorMessage = "El apellido no puede tener más de 20 caracteres.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El sueldo es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El sueldo debe ser un valor positivo.")]
        public int Sueldo { get; set; }
    }
}
