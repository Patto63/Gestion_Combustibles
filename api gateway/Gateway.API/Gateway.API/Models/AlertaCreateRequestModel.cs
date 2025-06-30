namespace Gateway.API.Models;

public class AlertaCreateRequestModel
{
    public string CodigoVehiculo { get; set; } = string.Empty;
    public string CodigoConductor { get; set; } = string.Empty;
    public string CodigoRuta { get; set; } = string.Empty;
    public int RegistroId { get; set; }
    public string TipoMaquinaria { get; set; } = string.Empty;
    public string TipoAlerta { get; set; } = string.Empty;
    public double PorcentajeDiferencia { get; set; }
    public bool Estado { get; set; }
    public string? Descripcion { get; set; }
}
