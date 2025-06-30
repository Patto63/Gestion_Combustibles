using AlertsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertsService.Persistence;

public class AlertsDbContext : DbContext
{
    public AlertsDbContext(DbContextOptions<AlertsDbContext> options) : base(options) { }

    public DbSet<AlertaConsumo> AlertasConsumo => Set<AlertaConsumo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertaConsumo>().ToTable("alertas_consumo");

        var alerta = modelBuilder.Entity<AlertaConsumo>();
        alerta.HasKey(a => a.AlertaId);
        alerta.Property(a => a.AlertaId).HasColumnName("alerta_id");
        alerta.Property(a => a.CodigoVehiculo).HasColumnName("codigo_vehiculo");
        alerta.Property(a => a.CodigoConductor).HasColumnName("codigo_conductor");
        alerta.Property(a => a.CodigoRuta).HasColumnName("codigo_ruta");
        alerta.Property(a => a.RegistroId).HasColumnName("registro_id");
        alerta.Property(a => a.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        alerta.Property(a => a.TipoAlerta).HasColumnName("tipo_alerta");
        alerta.Property(a => a.PorcentajeDiferencia).HasColumnName("porcentaje_diferencia");
        alerta.Property(a => a.Estado).HasColumnName("estado");
        alerta.Property(a => a.Descripcion).HasColumnName("descripcion");
        alerta.Property(a => a.CreadoEn).HasColumnName("creado_en");
        alerta.Property(a => a.RevisadoEn).HasColumnName("revisado_en");
        alerta.Property(a => a.RevisadoPor).HasColumnName("revisado_por");
    }
}
