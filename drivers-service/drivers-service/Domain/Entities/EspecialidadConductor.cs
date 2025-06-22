namespace DriversService.Domain.Entities;

public class EspecialidadConductor
{
    public int EspecialidadId { get; set; }
    public int ConductorId { get; set; }
    public required string TipoMaquinaria { get; set; }
    public string? Descripcion { get; set; }
    public string? NumeroCertificacion { get; set; }
    public DateTime FechaCertificacion { get; set; }
    public DateTime ExpiracionCertificacion { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public Conductor? Conductor { get; set; }
}
