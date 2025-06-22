namespace Gateway.API.Models;

public class ConductorCreateRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string TipoLicencia { get; set; } = string.Empty;
    public DateTime FechaExpiracionLicencia { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaIngreso { get; set; }
}
