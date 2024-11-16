using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstacionamiento.Models;

public partial class Ticket
{
    public decimal CodigoTicket { get; set; }

    public DateOnly FechaEntrada { get; set; }

    public TimeOnly HoraEntrada { get; set; }

    public string Matricula { get; set; } = null!;

    public decimal Dni { get; set; }

    public virtual Cliente DniNavigation { get; set; } = null!;

    public virtual Vehiculo MatriculaNavigation { get; set; } = null!;
}
