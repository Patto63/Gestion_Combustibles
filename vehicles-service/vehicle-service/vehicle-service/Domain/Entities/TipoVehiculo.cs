namespace VehicleService.Domain.Entities;

public class TipoVehiculo
{
    public int Id { get; set; }
    public required string Nombre { get; set; }

    public ICollection<Vehiculo>? Vehiculos { get; set; }
}
