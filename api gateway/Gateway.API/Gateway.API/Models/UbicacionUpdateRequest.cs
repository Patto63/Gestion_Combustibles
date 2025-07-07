namespace Gateway.API.Models;

public class UbicacionUpdateRequest
{
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public string? Pais { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Tipo { get; set; }
}
