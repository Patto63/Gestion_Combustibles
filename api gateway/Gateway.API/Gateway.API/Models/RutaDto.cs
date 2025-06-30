namespace Gateway.API.Models;

public class RutaDto
{
    public int RutaId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int OrigenId { get; set; }
    public int DestinoId { get; set; }
    public double Distancia { get; set; }
    public double TiempoEstimado { get; set; }
    public string? TipoTerreno { get; set; }
    public string? Descripcion { get; set; }
    public bool EstaActiva { get; set; }
}
