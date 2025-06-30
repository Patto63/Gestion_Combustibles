namespace AlertsService.Domain.Entities;

public class AlertaConsumo
{
    public int AlertaId { get; set; }
    public required string CodigoVehiculo { get; set; }
    public required string CodigoConductor { get; set; }
    public required string CodigoRuta { get; set; }
    public int RegistroId { get; set; }
    public required string TipoMaquinaria { get; set; }
    public required string TipoAlerta { get; set; }
    public decimal PorcentajeDiferencia { get; set; }
    public bool Estado { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? RevisadoEn { get; set; }
    public string? RevisadoPor { get; set; }
}
