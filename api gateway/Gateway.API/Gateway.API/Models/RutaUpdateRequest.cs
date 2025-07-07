namespace Gateway.API.Models;

public class RutaUpdateRequest
{
    public string? Codigo { get; set; }
    public string? Nombre { get; set; }
    public int? OrigenId { get; set; }
    public int? DestinoId { get; set; }
    public double? Distancia { get; set; }
    public double? TiempoEstimado { get; set; }
    public string? TipoTerreno { get; set; }
    public string? Descripcion { get; set; }
    public bool? EstaActiva { get; set; }
}
