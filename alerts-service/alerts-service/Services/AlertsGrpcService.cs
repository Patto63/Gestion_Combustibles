using AlertsService.Domain.Entities;
using AlertsService.Persistence;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace AlertsService.Services;

public class AlertsGrpcService : AlertsService.AlertsServiceBase
{
    private readonly AlertsDbContext _context;

    public AlertsGrpcService(AlertsDbContext context)
    {
        _context = context;
    }

    private static AlertaDto ToDto(AlertaConsumo a) => new()
    {
        AlertaId = a.AlertaId,
        CodigoVehiculo = a.CodigoVehiculo,
        CodigoConductor = a.CodigoConductor,
        CodigoRuta = a.CodigoRuta,
        RegistroId = a.RegistroId,
        TipoMaquinaria = a.TipoMaquinaria,
        TipoAlerta = a.TipoAlerta,
        PorcentajeDiferencia = (double)a.PorcentajeDiferencia,
        Estado = a.Estado,
        Descripcion = a.Descripcion ?? string.Empty,
        CreadoEn = Timestamp.FromDateTime(a.CreadoEn.ToUniversalTime()),
        RevisadoEn = a.RevisadoEn != null ? Timestamp.FromDateTime(a.RevisadoEn.Value.ToUniversalTime()) : null,
        RevisadoPor = a.RevisadoPor ?? string.Empty
    };

    public override async Task<AlertaDto> CrearAlerta(AlertaCreateRequest request, ServerCallContext context)
    {
        var entity = new AlertaConsumo
        {
            CodigoVehiculo = request.CodigoVehiculo,
            CodigoConductor = request.CodigoConductor,
            CodigoRuta = request.CodigoRuta,
            RegistroId = request.RegistroId,
            TipoMaquinaria = request.TipoMaquinaria,
            TipoAlerta = request.TipoAlerta,
            PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia,
            Estado = request.Estado,
            Descripcion = request.Descripcion,
            CreadoEn = DateTime.UtcNow
        };
        _context.AlertasConsumo.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<AlertaDto> ObtenerAlerta(AlertaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.AlertasConsumo.FindAsync(request.AlertaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Alerta no encontrada"));
        return ToDto(entity);
    }

    public override async Task<ListaAlertas> ListarAlertas(Empty request, ServerCallContext context)
    {
        var list = await _context.AlertasConsumo.ToListAsync();
        var res = new ListaAlertas();
        res.Alertas.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<AlertaDto> EditarAlerta(AlertaUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.AlertasConsumo.FindAsync(request.AlertaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Alerta no encontrada"));

        if (request.HasCodigoVehiculo) entity.CodigoVehiculo = request.CodigoVehiculo;
        if (request.HasCodigoConductor) entity.CodigoConductor = request.CodigoConductor;
        if (request.HasCodigoRuta) entity.CodigoRuta = request.CodigoRuta;
        if (request.HasRegistroId) entity.RegistroId = request.RegistroId;
        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasTipoAlerta) entity.TipoAlerta = request.TipoAlerta;
        if (request.HasPorcentajeDiferencia) entity.PorcentajeDiferencia = (decimal)request.PorcentajeDiferencia;
        if (request.HasEstado) entity.Estado = request.Estado;
        if (request.HasDescripcion) entity.Descripcion = request.Descripcion;
        if (request.RevisadoEn != null) entity.RevisadoEn = request.RevisadoEn.ToDateTime();
        if (request.HasRevisadoPor) entity.RevisadoPor = request.RevisadoPor;

        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public override async Task<Empty> EliminarAlerta(AlertaIdRequest request, ServerCallContext context)
    {
        var entity = await _context.AlertasConsumo.FindAsync(request.AlertaId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Alerta no encontrada"));
        _context.AlertasConsumo.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    public override async Task<ListaAlertas> AlertasPorVehiculo(CodigoVehiculoRequest request, ServerCallContext context)
    {
        var list = await _context.AlertasConsumo.Where(a => a.CodigoVehiculo == request.CodigoVehiculo).ToListAsync();
        var res = new ListaAlertas();
        res.Alertas.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<ListaAlertas> AlertasPorConductor(CodigoConductorRequest request, ServerCallContext context)
    {
        var list = await _context.AlertasConsumo.Where(a => a.CodigoConductor == request.CodigoConductor).ToListAsync();
        var res = new ListaAlertas();
        res.Alertas.AddRange(list.Select(ToDto));
        return res;
    }

    public override async Task<ListaAlertas> AlertasPorRuta(CodigoRutaRequest request, ServerCallContext context)
    {
        var list = await _context.AlertasConsumo.Where(a => a.CodigoRuta == request.CodigoRuta).ToListAsync();
        var res = new ListaAlertas();
        res.Alertas.AddRange(list.Select(ToDto));
        return res;
    }
}
