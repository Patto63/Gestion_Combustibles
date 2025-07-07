using FuelService.Domain.Entities;
using FuelService.Persistence;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace FuelService.Services;

public class FuelGrpcService : FuelService.FuelServiceBase
{
    private readonly FuelDbContext _context;

    public FuelGrpcService(FuelDbContext context)
    {
        _context = context;
    }

    // Helpers
    private static RegistroCombustibleDto ToDto(RegistroCombustible r) => new()
    {
        RegistroId = r.RegistroId,
        Fecha = Timestamp.FromDateTime(r.Fecha.ToUniversalTime()),
        CodigoVehiculo = r.CodigoVehiculo,
        CodigoConductor = r.CodigoConductor,
        CodigoRuta = r.CodigoRuta,
        TipoMaquinaria = r.TipoMaquinaria,
        OdometroInicial = (double)r.OdometroInicial,
        OdometroFinal = (double)r.OdometroFinal,
        Distancia = (double)r.Distancia,
        CantidadCombustible = (double)r.CantidadCombustible,
        PrecioCombustible = (double)r.PrecioCombustible,
        CostoTotal = (double)r.CostoTotal,
        ConsumoReal = (double)r.ConsumoReal,
        ConsumoEstimado = (double)r.ConsumoEstimado,
        Diferencia = (double)r.Diferencia,
        TipoCombustible = r.TipoCombustible,
        NumeroFactura = r.NumeroFactura ?? string.Empty,
        NombreEstacionServicio = r.NombreEstacionServicio ?? string.Empty,
        Comentarios = r.Comentarios ?? string.Empty,
        CreadoEn = Timestamp.FromDateTime(r.CreadoEn.ToUniversalTime()),
        CreadoPor = r.CreadoPor
    };

    private static ConsumoRutaDto ToDto(ConsumoCombustibleRuta c) => new()
    {
        ConsumoId = c.ConsumoId,
        CodigoRuta = c.CodigoRuta,
        Periodo = c.Periodo,
        TipoMaquinaria = c.TipoMaquinaria,
        TotalVehiculos = c.TotalVehiculos,
        DistanciaTotal = (double)c.DistanciaTotal,
        CombustibleTotal = (double)c.CombustibleTotal,
        CostoTotal = (double)c.CostoTotal,
        ConsumoPromedio = (double)c.ConsumoPromedio,
        ConsumoEstimado = (double)c.ConsumoEstimado,
        PorcentajeDiferencia = (double)c.PorcentajeDiferencia,
        CreadoEn = Timestamp.FromDateTime(c.CreadoEn.ToUniversalTime()),
        ActualizadoEn = c.ActualizadoEn != null ? Timestamp.FromDateTime(c.ActualizadoEn.Value.ToUniversalTime()) : null
    };

    private static ConsumoTipoMaquinariaDto ToDto(ConsumoCombustibleTipoMaquinaria c) => new()
    {
        ConsumoMaquinariaId = c.ConsumoMaquinariaId,
        TipoMaquinaria = c.TipoMaquinaria,
        Periodo = c.Periodo,
        TotalVehiculos = c.TotalVehiculos,
        DistanciaTotal = (double)c.DistanciaTotal,
        CombustibleTotal = (double)c.CombustibleTotal,
        CostoTotal = (double)c.CostoTotal,
        ConsumoPromedio = (double)c.ConsumoPromedio,
        ConsumoEstimado = (double)c.ConsumoEstimado,
        PorcentajeDiferencia = (double)c.PorcentajeDiferencia,
        CreadoEn = Timestamp.FromDateTime(c.CreadoEn.ToUniversalTime()),
        ActualizadoEn = c.ActualizadoEn != null ? Timestamp.FromDateTime(c.ActualizadoEn.Value.ToUniversalTime()) : null
    };

    private static TipoMaquinariaDto ToDto(TipoMaquinaria t) => new()
    {
        TipoMaquinariaId = t.TipoMaquinariaId,
        Nombre = t.Nombre
    };

    // Registros CRUD
    public override async Task<RegistroCombustibleDto> CrearRegistro(RegistroCombustibleCreateRequest request, ServerCallContext context)
    {
        var entity = new RegistroCombustible
        {
            Fecha = request.Fecha.ToDateTime(),
            CodigoVehiculo = request.CodigoVehiculo,
            CodigoConductor = request.CodigoConductor,
            CodigoRuta = request.CodigoRuta,
            TipoMaquinaria = request.TipoMaquinaria,
            OdometroInicial = (decimal)request.OdometroInicial,
            OdometroFinal = (decimal)request.OdometroFinal,
            Distancia = (decimal)request.Distancia,
            CantidadCombustible = (decimal)request.CantidadCombustible,
            PrecioCombustible = (decimal)request.PrecioCombustible,
            ConsumoEstimado = (decimal)request.ConsumoEstimado,
            TipoCombustible = request.TipoCombustible,
            NumeroFactura = request.NumeroFactura,
            NombreEstacionServicio = request.NombreEstacionServicio,
            Comentarios = request.Comentarios,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = request.CreadoPor,
            CostoTotal = (decimal)request.CantidadCombustible * (decimal)request.PrecioCombustible,
            ConsumoReal = 0,
            Diferencia = 0
        };
        _context.RegistrosCombustible.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<RegistroCombustibleDto> ObtenerRegistro(RegistroCombustibleIdRequest request, ServerCallContext context)
    {
        var entity = await _context.RegistrosCombustible.FindAsync(request.RegistroId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Registro no encontrado"));
        return ToDto(entity);
    }

    public override async Task<ListaRegistrosCombustible> ListarRegistros(Empty request, ServerCallContext context)
    {
        var list = await _context.RegistrosCombustible.ToListAsync();
        var res = new ListaRegistrosCombustible();
        res.Registros.AddRange(list.Select(ToDto));
        return res;
    }
    public override async Task<RegistroCombustibleDto> EditarRegistro(RegistroCombustibleUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.RegistrosCombustible.FindAsync(request.RegistroId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Registro no encontrado"));

        if (request.Fecha != null) entity.Fecha = request.Fecha.ToDateTime();
        if (request.HasCodigoVehiculo) entity.CodigoVehiculo = request.CodigoVehiculo;
        if (request.HasCodigoConductor) entity.CodigoConductor = request.CodigoConductor;
        if (request.HasCodigoRuta) entity.CodigoRuta = request.CodigoRuta;
        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasOdometroInicial) entity.OdometroInicial = (decimal)request.OdometroInicial;
        if (request.HasOdometroFinal) entity.OdometroFinal = (decimal)request.OdometroFinal;
        if (request.HasDistancia) entity.Distancia = (decimal)request.Distancia;
        if (request.HasCantidadCombustible) entity.CantidadCombustible = (decimal)request.CantidadCombustible;
        if (request.HasPrecioCombustible) entity.PrecioCombustible = (decimal)request.PrecioCombustible;
        if (request.HasConsumoEstimado) entity.ConsumoEstimado = (decimal)request.ConsumoEstimado;
        if (request.HasTipoCombustible) entity.TipoCombustible = request.TipoCombustible;
        if (request.HasNumeroFactura) entity.NumeroFactura = request.NumeroFactura;
        if (request.HasNombreEstacionServicio) entity.NombreEstacionServicio = request.NombreEstacionServicio;
        if (request.HasComentarios) entity.Comentarios = request.Comentarios;

        entity.CostoTotal = entity.CantidadCombustible * entity.PrecioCombustible;
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<Empty> EliminarRegistro(RegistroCombustibleIdRequest request, ServerCallContext context)
    {
        var entity = await _context.RegistrosCombustible.FindAsync(request.RegistroId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Registro no encontrado"));
        _context.RegistrosCombustible.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    // Consumo Ruta CRUD
    public override async Task<ConsumoRutaDto> CrearConsumoRuta(ConsumoRutaCreateRequest request, ServerCallContext context)
    {
        var entity = new ConsumoCombustibleRuta
        {
            CodigoRuta = request.CodigoRuta,
            Periodo = request.Periodo,
            TipoMaquinaria = request.TipoMaquinaria,
            TotalVehiculos = request.TotalVehiculos,
            DistanciaTotal = (decimal)request.DistanciaTotal,
            CombustibleTotal = (decimal)request.CombustibleTotal,
            CostoTotal = (decimal)request.CostoTotal,
            ConsumoPromedio = (decimal)request.ConsumoPromedio,
            ConsumoEstimado = (decimal)request.ConsumoEstimado,
            PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia,
            CreadoEn = DateTime.UtcNow
        };
        _context.ConsumosRuta.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<ConsumoRutaDto> ObtenerConsumoRuta(ConsumoRutaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosRuta.FindAsync(request.ConsumoId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));
        return ToDto(entity);
    }

    public override async Task<ListaConsumoRuta> ListarConsumoRuta(Empty request, ServerCallContext context)
    {
        var list = await _context.ConsumosRuta.ToListAsync();
        var res = new ListaConsumoRuta();
        res.Consumos.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<ConsumoRutaDto> EditarConsumoRuta(ConsumoRutaUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosRuta.FindAsync(request.ConsumoId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));

        if (request.HasCodigoRuta) entity.CodigoRuta = request.CodigoRuta;
        if (request.HasPeriodo) entity.Periodo = request.Periodo;
        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasTotalVehiculos) entity.TotalVehiculos = request.TotalVehiculos;
        if (request.HasDistanciaTotal) entity.DistanciaTotal = (decimal)request.DistanciaTotal;
        if (request.HasCombustibleTotal) entity.CombustibleTotal = (decimal)request.CombustibleTotal;
        if (request.HasCostoTotal) entity.CostoTotal = (decimal)request.CostoTotal;
        if (request.HasConsumoPromedio) entity.ConsumoPromedio = (decimal)request.ConsumoPromedio;
        if (request.HasConsumoEstimado) entity.ConsumoEstimado = (decimal)request.ConsumoEstimado;
        if (request.HasPorcentajeDiferencia) entity.PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<Empty> EliminarConsumoRuta(ConsumoRutaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosRuta.FindAsync(request.ConsumoId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));
        _context.ConsumosRuta.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    // Consumo Tipo Maquinaria CRUD
    public override async Task<ConsumoTipoMaquinariaDto> CrearConsumoTipoMaquinaria(ConsumoTipoMaquinariaCreateRequest request, ServerCallContext context)
    {
        var entity = new ConsumoCombustibleTipoMaquinaria
        {
            TipoMaquinaria = request.TipoMaquinaria,
            Periodo = request.Periodo,
            TotalVehiculos = request.TotalVehiculos,
            DistanciaTotal = (decimal)request.DistanciaTotal,
            CombustibleTotal = (decimal)request.CombustibleTotal,
            CostoTotal = (decimal)request.CostoTotal,
            ConsumoPromedio = (decimal)request.ConsumoPromedio,
            ConsumoEstimado = (decimal)request.ConsumoEstimado,
            PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia,
            CreadoEn = DateTime.UtcNow
        };
        _context.ConsumosTipoMaquinaria.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<ConsumoTipoMaquinariaDto> ObtenerConsumoTipoMaquinaria(ConsumoTipoMaquinariaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosTipoMaquinaria.FindAsync(request.ConsumoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));
        return ToDto(entity);
    }

    public override async Task<ListaConsumoTipoMaquinaria> ListarConsumoTipoMaquinaria(Empty request, ServerCallContext context)
    {
        var list = await _context.ConsumosTipoMaquinaria.ToListAsync();
        var res = new ListaConsumoTipoMaquinaria();
        res.Consumos.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<ConsumoTipoMaquinariaDto> EditarConsumoTipoMaquinaria(ConsumoTipoMaquinariaUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosTipoMaquinaria.FindAsync(request.ConsumoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));

        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasPeriodo) entity.Periodo = request.Periodo;
        if (request.HasTotalVehiculos) entity.TotalVehiculos = request.TotalVehiculos;
        if (request.HasDistanciaTotal) entity.DistanciaTotal = (decimal)request.DistanciaTotal;
        if (request.HasCombustibleTotal) entity.CombustibleTotal = (decimal)request.CombustibleTotal;
        if (request.HasCostoTotal) entity.CostoTotal = (decimal)request.CostoTotal;
        if (request.HasConsumoPromedio) entity.ConsumoPromedio = (decimal)request.ConsumoPromedio;
        if (request.HasConsumoEstimado) entity.ConsumoEstimado = (decimal)request.ConsumoEstimado;
        if (request.HasPorcentajeDiferencia) entity.PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<Empty> EliminarConsumoTipoMaquinaria(ConsumoTipoMaquinariaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.ConsumosTipoMaquinaria.FindAsync(request.ConsumoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Consumo no encontrado"));
        _context.ConsumosTipoMaquinaria.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    // Tipo Maquinaria CRUD
    public override async Task<TipoMaquinariaDto> CrearTipoMaquinaria(TipoMaquinariaCreateRequest request, ServerCallContext context)
    {
        var entity = new TipoMaquinaria { Nombre = request.Nombre };
        _context.TiposMaquinaria.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<TipoMaquinariaDto> ObtenerTipoMaquinaria(TipoMaquinariaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.TiposMaquinaria.FindAsync(request.TipoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Tipo no encontrado"));
        return ToDto(entity);
    }

    public override async Task<ListaTipoMaquinaria> ListarTiposMaquinaria(Empty request, ServerCallContext context)
    {
        var list = await _context.TiposMaquinaria.ToListAsync();
        var res = new ListaTipoMaquinaria();
        res.Tipos.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<TipoMaquinariaDto> EditarTipoMaquinaria(TipoMaquinariaUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.TiposMaquinaria.FindAsync(request.TipoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Tipo no encontrado"));
        if (request.HasNombre) entity.Nombre = request.Nombre;
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<Empty> EliminarTipoMaquinaria(TipoMaquinariaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.TiposMaquinaria.FindAsync(request.TipoMaquinariaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Tipo no encontrado"));
        _context.TiposMaquinaria.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }
}
