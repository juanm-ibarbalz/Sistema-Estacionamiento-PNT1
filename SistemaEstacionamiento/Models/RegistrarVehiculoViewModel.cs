using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SistemaEstacionamiento.Models
{
    public class RegistrarVehiculoViewModel
    {
        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        [StringLength(7, ErrorMessage = "La matrícula no puede tener más de 7 caracteres.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "La matrícula solo puede contener letras y números.")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        public string Tipo { get; set; }

        [StringLength(20, ErrorMessage = "El color no puede tener más de 20 caracteres.")]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "El color solo puede contener letras.")]
        public string? Color { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El piso debe ser un valor positivo.")]
        public int? Piso { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El lugar debe ser un valor positivo.")]
        public int? Lugar { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El DNI solo puede contener números.")]
        public decimal Dni { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre no puede tener más de 20 caracteres.")]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "El nombre solo puede contener letras.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(20, ErrorMessage = "El apellido no puede tener más de 20 caracteres.")]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "El apellido solo puede contener letras.")]
        public string Apellido { get; set; }
    }
}
