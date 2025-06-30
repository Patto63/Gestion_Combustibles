namespace FuelService.Domain.Entities;

public class ConsumoCombustibleRuta
{
    public int ConsumoId { get; set; }
    public required string CodigoRuta { get; set; }
    public required string Periodo { get; set; }
    public required string TipoMaquinaria { get; set; }
    public int TotalVehiculos { get; set; }
    public decimal DistanciaTotal { get; set; }
    public decimal CombustibleTotal { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal ConsumoPromedio { get; set; }
    public decimal ConsumoEstimado { get; set; }
    public decimal PorcentajeDiferencia { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
}
