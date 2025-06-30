namespace FuelService.Domain.Entities;

public class RegistroCombustible
{
    public int RegistroId { get; set; }
    public DateTime Fecha { get; set; }
    public required string CodigoVehiculo { get; set; }
    public required string CodigoConductor { get; set; }
    public required string CodigoRuta { get; set; }
    public required string TipoMaquinaria { get; set; }
    public decimal OdometroInicial { get; set; }
    public decimal OdometroFinal { get; set; }
    public decimal Distancia { get; set; }
    public decimal CantidadCombustible { get; set; }
    public decimal PrecioCombustible { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal ConsumoReal { get; set; }
    public decimal ConsumoEstimado { get; set; }
    public decimal Diferencia { get; set; }
    public required string TipoCombustible { get; set; }
    public string? NumeroFactura { get; set; }
    public string? NombreEstacionServicio { get; set; }
    public string? Comentarios { get; set; }
    public DateTime CreadoEn { get; set; }
    public required string CreadoPor { get; set; }
}
