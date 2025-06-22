namespace Gateway.API.Models;

public class VehiculoUpdateRequest
{
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? Anio { get; set; }
    public int? TipoMaquinariaId { get; set; }
    public bool? EstadoOperativo { get; set; }
    public double? CapacidadTanqueGalones { get; set; }
    public double? CombustibleActualGalones { get; set; }
}
