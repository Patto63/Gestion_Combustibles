namespace Gateway.API.Models;

public class SegmentoUpdateRequest
{
    public int? RutaId { get; set; }
    public int? NumeroSecuencia { get; set; }
    public int? UbicacionInicioId { get; set; }
    public int? UbicacionFinId { get; set; }
    public double? DistanciaSegmento { get; set; }
    public double? TiempoSegmento { get; set; }
    public string? TipoTerreno { get; set; }
    public string? Descripcion { get; set; }
}
