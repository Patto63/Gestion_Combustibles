namespace Gateway.API.Models;

public class ConductorUpdateRequest
{
    public string? Codigo { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? NumeroLicencia { get; set; }
    public string? TipoLicencia { get; set; }
    public DateTime? FechaExpiracionLicencia { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
    public bool? Estado { get; set; }
}
