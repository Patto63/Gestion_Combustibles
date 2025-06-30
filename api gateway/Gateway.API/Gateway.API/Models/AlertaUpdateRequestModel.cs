namespace Gateway.API.Models;

public class AlertaUpdateRequestModel
{
    public string? CodigoVehiculo { get; set; }
    public string? CodigoConductor { get; set; }
    public string? CodigoRuta { get; set; }
    public int? RegistroId { get; set; }
    public string? TipoMaquinaria { get; set; }
    public string? TipoAlerta { get; set; }
    public double? PorcentajeDiferencia { get; set; }
    public bool? Estado { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? RevisadoEn { get; set; }
    public string? RevisadoPor { get; set; }
}
