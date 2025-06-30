namespace RoutesService.Domain.Entities;

public class SegmentoRuta
{
    public int SegmentoId { get; set; }
    public int RutaId { get; set; }
    public int NumeroSecuencia { get; set; }
    public int UbicacionInicioId { get; set; }
    public int UbicacionFinId { get; set; }
    public decimal DistanciaSegmento { get; set; }
    public decimal TiempoSegmento { get; set; }
    public string? TipoTerreno { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }

    public Ruta? Ruta { get; set; }
}
