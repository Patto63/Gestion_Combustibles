using Microsoft.EntityFrameworkCore;
using RoutesService.Domain.Entities;

namespace RoutesService.Persistence;

public class RoutesDbContext : DbContext
{
    public RoutesDbContext(DbContextOptions<RoutesDbContext> options) : base(options) { }

    public DbSet<Ruta> Rutas => Set<Ruta>();
    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();
    public DbSet<SegmentoRuta> Segmentos => Set<SegmentoRuta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ruta>().ToTable("rutas");
        modelBuilder.Entity<Ubicacion>().ToTable("ubicaciones");
        modelBuilder.Entity<SegmentoRuta>().ToTable("segmentos_ruta");

        var ruta = modelBuilder.Entity<Ruta>();
        ruta.HasKey(r => r.RutaId);
        ruta.Property(r => r.RutaId).HasColumnName("ruta_id");
        ruta.Property(r => r.Codigo).HasColumnName("codigo");
        ruta.Property(r => r.Nombre).HasColumnName("nombre");
        ruta.Property(r => r.OrigenId).HasColumnName("origen_id");
        ruta.Property(r => r.DestinoId).HasColumnName("destino_id");
        ruta.Property(r => r.Distancia).HasColumnName("distancia");
        ruta.Property(r => r.TiempoEstimado).HasColumnName("tiempo_estimado");
        ruta.Property(r => r.TipoTerreno).HasColumnName("tipo_terreno");
        ruta.Property(r => r.Descripcion).HasColumnName("descripcion");
        ruta.Property(r => r.EstaActiva).HasColumnName("esta_activa");
        ruta.Property(r => r.CreadoEn).HasColumnName("creado_en");
        ruta.Property(r => r.ActualizadoEn).HasColumnName("actualizado_en");

        var ubi = modelBuilder.Entity<Ubicacion>();
        ubi.HasKey(u => u.UbicacionId);
        ubi.Property(u => u.UbicacionId).HasColumnName("ubicacion_id");
        ubi.Property(u => u.Nombre).HasColumnName("nombre");
        ubi.Property(u => u.Direccion).HasColumnName("direccion");
        ubi.Property(u => u.Ciudad).HasColumnName("ciudad");
        ubi.Property(u => u.Estado).HasColumnName("estado");
        ubi.Property(u => u.Pais).HasColumnName("pais");
        ubi.Property(u => u.Latitud).HasColumnName("latitud");
        ubi.Property(u => u.Longitud).HasColumnName("longitud");
        ubi.Property(u => u.Tipo).HasColumnName("tipo");
        ubi.Property(u => u.CreadoEn).HasColumnName("creado_en");
        ubi.Property(u => u.ActualizadoEn).HasColumnName("actualizado_en");

        var seg = modelBuilder.Entity<SegmentoRuta>();
        seg.HasKey(s => s.SegmentoId);
        seg.Property(s => s.SegmentoId).HasColumnName("segmento_id");
        seg.Property(s => s.RutaId).HasColumnName("ruta_id");
        seg.Property(s => s.NumeroSecuencia).HasColumnName("numero_secuencia");
        seg.Property(s => s.UbicacionInicioId).HasColumnName("ubicacion_inicio_id");
        seg.Property(s => s.UbicacionFinId).HasColumnName("ubicacion_fin_id");
        seg.Property(s => s.DistanciaSegmento).HasColumnName("distancia_segmento");
        seg.Property(s => s.TiempoSegmento).HasColumnName("tiempo_segmento");
        seg.Property(s => s.TipoTerreno).HasColumnName("tipo_terreno");
        seg.Property(s => s.Descripcion).HasColumnName("descripcion");
        seg.Property(s => s.CreadoEn).HasColumnName("creado_en");
        seg.Property(s => s.ActualizadoEn).HasColumnName("actualizado_en");
        seg.HasOne(s => s.Ruta).WithMany(r => r.Segmentos).HasForeignKey(s => s.RutaId);
    }
}
