using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleService.Domain.Entities;

namespace VehicleService.Persistence
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }

        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<TipoVehiculo> TipoVehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoVehiculo>().ToTable("tipo_vehiculo");
            modelBuilder.Entity<Vehiculo>().ToTable("vehiculos");

            modelBuilder.Entity<Vehiculo>().Property(v => v.Id).HasColumnName("id");
            modelBuilder.Entity<Vehiculo>().Property(v => v.Placa).HasColumnName("placa");
            modelBuilder.Entity<Vehiculo>().Property(v => v.Marca).HasColumnName("marca");
            modelBuilder.Entity<Vehiculo>().Property(v => v.Modelo).HasColumnName("modelo");
            modelBuilder.Entity<Vehiculo>().Property(v => v.Anio).HasColumnName("anio");
            modelBuilder.Entity<Vehiculo>().Property(v => v.TipoVehiculoId).HasColumnName("tipo_vehiculo_id");
            modelBuilder.Entity<Vehiculo>().Property(v => v.EstadoOperativo).HasColumnName("estado_operativo");
            modelBuilder.Entity<Vehiculo>().Property(v => v.CapacidadTanqueGalones).HasColumnName("capacidad_tanque_galones");
            modelBuilder.Entity<Vehiculo>().Property(v => v.CombustibleActualGalones).HasColumnName("combustible_actual_galones");

            modelBuilder.Entity<TipoVehiculo>().Property(t => t.Id).HasColumnName("id");
            modelBuilder.Entity<TipoVehiculo>().Property(t => t.Nombre).HasColumnName("nombre");

            modelBuilder.Entity<TipoVehiculo>()
                .HasMany(t => t.Vehiculos)
                .WithOne(v => v.TipoVehiculo)
                .HasForeignKey(v => v.TipoVehiculoId);
        }

    }
}
