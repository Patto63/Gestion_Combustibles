namespace DriversService.Domain.Entities;

public class HistorialAsignacionConductor
{
    public int AsignacionId { get; set; }
    public int ConductorId { get; set; }
    public required string CodigoVehiculo { get; set; }
    public required string TipoMaquinaria { get; set; }
    public DateTime FechaInicioAsignacion { get; set; }
    public DateTime? FechaFinAsignacion { get; set; }
    public string? Estado { get; set; }
    public DateTime CreadoEn { get; set; }
    public string? CreadoPor { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public Conductor? Conductor { get; set; }
}
