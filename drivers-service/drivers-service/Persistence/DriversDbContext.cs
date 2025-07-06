using Microsoft.EntityFrameworkCore;
using DriversService.Domain.Entities;

namespace DriversService.Persistence;

public class DriversDbContext : DbContext
{
    public DriversDbContext(DbContextOptions<DriversDbContext> options) : base(options) { }

    public DbSet<Conductor> Conductores => Set<Conductor>();
    public DbSet<EspecialidadConductor> Especialidades => Set<EspecialidadConductor>();
    public DbSet<HistorialAsignacionConductor> Asignaciones => Set<HistorialAsignacionConductor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conductor>().ToTable("conductores");
        modelBuilder.Entity<EspecialidadConductor>().ToTable("especialidades_conductor");
        modelBuilder.Entity<HistorialAsignacionConductor>().ToTable("historial_asignacion_conductor");

        // Conductor mapping
        var conductor = modelBuilder.Entity<Conductor>();
        conductor.HasKey(c => c.ConductorId);
        conductor.Property(c => c.ConductorId).HasColumnName("conductor_id");
        conductor.Property(c => c.Codigo).HasColumnName("codigo");
        conductor.Property(c => c.Nombre).HasColumnName("nombre");
        conductor.Property(c => c.Apellido).HasColumnName("apellido");
        conductor.Property(c => c.NumeroDocumento).HasColumnName("numero_documento");
        conductor.Property(c => c.NumeroLicencia).HasColumnName("numero_licencia");
        conductor.Property(c => c.TipoLicencia).HasColumnName("tipo_licencia");
        conductor.Property(c => c.FechaExpiracionLicencia).HasColumnName("fecha_expiracion_licencia");
        conductor.Property(c => c.FechaNacimiento).HasColumnName("fecha_nacimiento");
        conductor.Property(c => c.NumeroTelefono).HasColumnName("numero_telefono");
        conductor.Property(c => c.CorreoElectronico).HasColumnName("correo_electronico");
        conductor.Property(c => c.Direccion).HasColumnName("direccion");
        conductor.Property(c => c.FechaIngreso).HasColumnName("fecha_ingreso");
        conductor.Property(c => c.Estado).HasColumnName("estado");
        conductor.Property(c => c.CreadoEn).HasColumnName("creado_en");
        conductor.Property(c => c.ActualizadoEn).HasColumnName("actualizado_en");

        // Especialidad
        var esp = modelBuilder.Entity<EspecialidadConductor>();
        esp.HasKey(e => e.EspecialidadId);
        esp.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");
        esp.Property(e => e.ConductorId).HasColumnName("conductor_id");
        esp.Property(e => e.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        esp.Property(e => e.Descripcion).HasColumnName("descripcion");
        esp.Property(e => e.NumeroCertificacion).HasColumnName("numero_certificacion");
        esp.Property(e => e.FechaCertificacion).HasColumnName("fecha_certificacion");
        esp.Property(e => e.ExpiracionCertificacion).HasColumnName("expiracion_certificacion");
        esp.Property(e => e.CreadoEn).HasColumnName("creado_en");
        esp.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en");
        esp.HasOne(e => e.Conductor).WithMany(c => c.Especialidades).HasForeignKey(e => e.ConductorId);

        // Asignacion
        var asig = modelBuilder.Entity<HistorialAsignacionConductor>();
        asig.HasKey(a => a.AsignacionId);
        asig.Property(a => a.AsignacionId).HasColumnName("asignacion_id");
        asig.Property(a => a.ConductorId).HasColumnName("conductor_id");
        asig.Property(a => a.CodigoVehiculo).HasColumnName("codigo_vehiculo");
        asig.Property(a => a.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        asig.Property(a => a.FechaInicioAsignacion).HasColumnName("fecha_inicio_asignacion");
        asig.Property(a => a.FechaFinAsignacion).HasColumnName("fecha_fin_asignacion");
        asig.Property(a => a.Estado).HasColumnName("estado");
        asig.Property(a => a.CreadoEn).HasColumnName("creado_en");
        asig.Property(a => a.CreadoPor).HasColumnName("creado_por");
        asig.Property(a => a.ActualizadoEn).HasColumnName("actualizado_en");
        asig.HasOne(a => a.Conductor).WithMany(c => c.Asignaciones).HasForeignKey(a => a.ConductorId);
    }
}
