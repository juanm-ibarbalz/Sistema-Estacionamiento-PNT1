﻿using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.ViewModels
{
    public class RetirarVehiculoViewModel
    {
        [Required(ErrorMessage = "El código de ticket es obligatorio.")]
        public decimal CodigoTicket { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(0, 99999999, ErrorMessage = "El DNI debe contener hasta 8 caracteres.")]
        public decimal Dni { get; set; }
    }
}

