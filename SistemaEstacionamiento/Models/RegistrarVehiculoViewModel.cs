using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models
{
    public class RegistrarVehiculoViewModel
    {
        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        [StringLength(7, ErrorMessage = "La matrícula no puede tener más de 7 caracteres.")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        public string Tipo { get; set; }

        [StringLength(20, ErrorMessage = "El color no puede tener más de 20 caracteres.")]
        public string? Color { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El piso debe ser un valor positivo.")]
        public byte? Piso { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El lugar debe ser un valor positivo.")]
        public byte? Lugar { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(0, 99999999, ErrorMessage = "El DNI puede contener hasta 8 caracteres.")]
        public decimal Dni { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre no puede tener más de 20 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(20, ErrorMessage = "El apellido no puede tener más de 20 caracteres.")]
        public string Apellido { get; set; }
    }
}
