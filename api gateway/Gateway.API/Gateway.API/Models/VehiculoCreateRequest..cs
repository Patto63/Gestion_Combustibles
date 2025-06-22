namespace Gateway.API.Models;

public class VehiculoCreateRequest
{
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public int TipoMaquinariaId { get; set; }
    public double CapacidadTanqueGalones { get; set; }
    public double CombustibleActualGalones { get; set; }
}
