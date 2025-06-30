namespace Gateway.API.Models;

public class RegistroCombustibleDto
{
    public int RegistroId { get; set; }
    public DateTime Fecha { get; set; }
    public string CodigoVehiculo { get; set; } = string.Empty;
    public string CodigoConductor { get; set; } = string.Empty;
    public string CodigoRuta { get; set; } = string.Empty;
    public string TipoMaquinaria { get; set; } = string.Empty;
    public double CantidadCombustible { get; set; }
    public double PrecioCombustible { get; set; }
    public double CostoTotal { get; set; }
}
