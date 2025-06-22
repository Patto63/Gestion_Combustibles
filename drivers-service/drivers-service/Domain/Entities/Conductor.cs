namespace DriversService.Domain.Entities;

public class Conductor
{
    public int ConductorId { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string NumeroDocumento { get; set; }
    public required string NumeroLicencia { get; set; }
    public required string TipoLicencia { get; set; }
    public DateTime FechaExpiracionLicencia { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaIngreso { get; set; }
    public bool Estado { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public ICollection<EspecialidadConductor>? Especialidades { get; set; }
    public ICollection<HistorialAsignacionConductor>? Asignaciones { get; set; }
}
