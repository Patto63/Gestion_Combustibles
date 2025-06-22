namespace Gateway.API.Models;

public class VehiculoFiltroRequest
{
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? Anio { get; set; }
    public int? TipoMaquinariaId { get; set; }
    public bool? EstadoOperativo { get; set; }
    public double? CapacidadMinima { get; set; }
    public double? CombustibleMinimo { get; set; }
}
