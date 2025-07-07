namespace VehicleService.Domain.Entities;

public class Vehiculo
{
    public int Id { get; set; }
    public required string Placa { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public required int Anio { get; set; }
    public required int TipoMaquinariaId { get; set; }
    public required bool EstadoOperativo { get; set; }
    public required decimal CapacidadTanqueGalones { get; set; }
    public required decimal CombustibleActualGalones { get; set; }

    public TipoMaquinaria TipoMaquinaria { get; set; } = null!;
}
