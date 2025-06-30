using Microsoft.EntityFrameworkCore;
using FuelService.Domain.Entities;

namespace FuelService.Persistence;

public class FuelDbContext : DbContext
{
    public FuelDbContext(DbContextOptions<FuelDbContext> options) : base(options) { }

    public DbSet<RegistroCombustible> RegistrosCombustible => Set<RegistroCombustible>();
    public DbSet<ConsumoCombustibleRuta> ConsumosRuta => Set<ConsumoCombustibleRuta>();
    public DbSet<ConsumoCombustibleTipoMaquinaria> ConsumosTipoMaquinaria => Set<ConsumoCombustibleTipoMaquinaria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegistroCombustible>().ToTable("registros_combustible");
        modelBuilder.Entity<ConsumoCombustibleRuta>().ToTable("consumo_combustible_ruta");
        modelBuilder.Entity<ConsumoCombustibleTipoMaquinaria>().ToTable("consumo_combustible_tipo_maquinaria");

        var registro = modelBuilder.Entity<RegistroCombustible>();
        registro.HasKey(r => r.RegistroId);
        registro.Property(r => r.RegistroId).HasColumnName("registro_id");
        registro.Property(r => r.Fecha).HasColumnName("fecha");
        registro.Property(r => r.CodigoVehiculo).HasColumnName("codigo_vehiculo");
        registro.Property(r => r.CodigoConductor).HasColumnName("codigo_conductor");
        registro.Property(r => r.CodigoRuta).HasColumnName("codigo_ruta");
        registro.Property(r => r.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        registro.Property(r => r.OdometroInicial).HasColumnName("odometro_inicial");
        registro.Property(r => r.OdometroFinal).HasColumnName("odometro_final");
        registro.Property(r => r.Distancia).HasColumnName("distancia");
        registro.Property(r => r.CantidadCombustible).HasColumnName("cantidad_combustible");
        registro.Property(r => r.PrecioCombustible).HasColumnName("precio_combustible");
        registro.Property(r => r.CostoTotal).HasColumnName("costo_total");
        registro.Property(r => r.ConsumoReal).HasColumnName("consumo_real");
        registro.Property(r => r.ConsumoEstimado).HasColumnName("consumo_estimado");
        registro.Property(r => r.Diferencia).HasColumnName("diferencia");
        registro.Property(r => r.TipoCombustible).HasColumnName("tipo_combustible");
        registro.Property(r => r.NumeroFactura).HasColumnName("numero_factura");
        registro.Property(r => r.NombreEstacionServicio).HasColumnName("nombre_estacion_servicio");
        registro.Property(r => r.Comentarios).HasColumnName("comentarios");
        registro.Property(r => r.CreadoEn).HasColumnName("creado_en");
        registro.Property(r => r.CreadoPor).HasColumnName("creado_por");

        var ruta = modelBuilder.Entity<ConsumoCombustibleRuta>();
        ruta.HasKey(r => r.ConsumoId);
        ruta.Property(r => r.ConsumoId).HasColumnName("consumo_id");
        ruta.Property(r => r.CodigoRuta).HasColumnName("codigo_ruta");
        ruta.Property(r => r.Periodo).HasColumnName("periodo");
        ruta.Property(r => r.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        ruta.Property(r => r.TotalVehiculos).HasColumnName("total_vehiculos");
        ruta.Property(r => r.DistanciaTotal).HasColumnName("distancia_total");
        ruta.Property(r => r.CombustibleTotal).HasColumnName("combustible_total");
        ruta.Property(r => r.CostoTotal).HasColumnName("costo_total");
        ruta.Property(r => r.ConsumoPromedio).HasColumnName("consumo_promedio");
        ruta.Property(r => r.ConsumoEstimado).HasColumnName("consumo_estimado");
        ruta.Property(r => r.PorcentajeDiferencia).HasColumnName("porcentaje_diferencia");
        ruta.Property(r => r.CreadoEn).HasColumnName("creado_en");
        ruta.Property(r => r.ActualizadoEn).HasColumnName("actualizado_en");

        var maq = modelBuilder.Entity<ConsumoCombustibleTipoMaquinaria>();
        maq.HasKey(m => m.ConsumoMaquinariaId);
        maq.Property(m => m.ConsumoMaquinariaId).HasColumnName("consumo_maquinaria_id");
        maq.Property(m => m.TipoMaquinaria).HasColumnName("tipo_maquinaria");
        maq.Property(m => m.Periodo).HasColumnName("periodo");
        maq.Property(m => m.TotalVehiculos).HasColumnName("total_vehiculos");
        maq.Property(m => m.DistanciaTotal).HasColumnName("distancia_total");
        maq.Property(m => m.CombustibleTotal).HasColumnName("combustible_total");
        maq.Property(m => m.CostoTotal).HasColumnName("costo_total");
        maq.Property(m => m.ConsumoPromedio).HasColumnName("consumo_promedio");
        maq.Property(m => m.ConsumoEstimado).HasColumnName("consumo_estimado");
        maq.Property(m => m.PorcentajeDiferencia).HasColumnName("porcentaje_diferencia");
        maq.Property(m => m.CreadoEn).HasColumnName("creado_en");
        maq.Property(m => m.ActualizadoEn).HasColumnName("actualizado_en");
    }
}
