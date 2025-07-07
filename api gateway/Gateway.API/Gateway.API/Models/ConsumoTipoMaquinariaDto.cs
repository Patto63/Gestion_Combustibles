namespace Gateway.API.Models;

public class ConsumoTipoMaquinariaDto
{
    public int ConsumoMaquinariaId { get; set; }
    public string TipoMaquinaria { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public int TotalVehiculos { get; set; }
    public double DistanciaTotal { get; set; }
    public double CombustibleTotal { get; set; }
    public double CostoTotal { get; set; }
    public double ConsumoPromedio { get; set; }
    public double ConsumoEstimado { get; set; }
    public double PorcentajeDiferencia { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime ActualizadoEn { get; set; }
}
