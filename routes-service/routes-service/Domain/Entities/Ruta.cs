namespace RoutesService.Domain.Entities;

public class Ruta
{
    public int RutaId { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public int OrigenId { get; set; }
    public int DestinoId { get; set; }
    public decimal Distancia { get; set; }
    public decimal TiempoEstimado { get; set; }
    public string? TipoTerreno { get; set; }
    public string? Descripcion { get; set; }
    public bool EstaActiva { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public ICollection<SegmentoRuta>? Segmentos { get; set; }
}
