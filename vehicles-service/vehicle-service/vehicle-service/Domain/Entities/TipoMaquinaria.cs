namespace VehicleService.Domain.Entities;

public class TipoMaquinaria
{
    public int TipoMaquinariaId { get; set; }
    public required string Nombre { get; set; }

    public ICollection<Vehiculo>? Vehiculos { get; set; }
}
