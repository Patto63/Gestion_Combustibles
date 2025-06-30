using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RoutesService.Domain.Entities;
using RoutesService.Persistence;

namespace RoutesService.Services;

public class RouteGrpcService : RouteService.RouteServiceBase
{
    private readonly RoutesDbContext _context;

    public RouteGrpcService(RoutesDbContext context)
    {
        _context = context;
    }

    private static RutaDto ToRutaDto(Ruta r) => new()
    {
        RutaId = r.RutaId,
        Codigo = r.Codigo,
        Nombre = r.Nombre,
        OrigenId = r.OrigenId,
        DestinoId = r.DestinoId,
        Distancia = (double)r.Distancia,
        TiempoEstimado = (double)r.TiempoEstimado,
        TipoTerreno = r.TipoTerreno ?? string.Empty,
        Descripcion = r.Descripcion ?? string.Empty,
        EstaActiva = r.EstaActiva
    };

    private static UbicacionDto ToUbicacionDto(Ubicacion u) => new()
    {
        UbicacionId = u.UbicacionId,
        Nombre = u.Nombre,
        Direccion = u.Direccion ?? string.Empty,
        Ciudad = u.Ciudad ?? string.Empty,
        Estado = u.Estado ?? string.Empty,
        Pais = u.Pais ?? string.Empty,
        Latitud = (double)(u.Latitud ?? 0),
        Longitud = (double)(u.Longitud ?? 0),
        Tipo = u.Tipo ?? string.Empty
    };

    private static SegmentoDto ToSegmentoDto(SegmentoRuta s) => new()
    {
        SegmentoId = s.SegmentoId,
        RutaId = s.RutaId,
        NumeroSecuencia = s.NumeroSecuencia,
        UbicacionInicioId = s.UbicacionInicioId,
        UbicacionFinId = s.UbicacionFinId,
        DistanciaSegmento = (double)s.DistanciaSegmento,
        TiempoSegmento = (double)s.TiempoSegmento,
        TipoTerreno = s.TipoTerreno ?? string.Empty,
        Descripcion = s.Descripcion ?? string.Empty
    };

    public override async Task<RutaDto> CrearRuta(RutaCreateRequest request, ServerCallContext context)
    {
        var ruta = new Ruta
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            OrigenId = request.OrigenId,
            DestinoId = request.DestinoId,
            Distancia = (decimal)request.Distancia,
            TiempoEstimado = (decimal)request.TiempoEstimado,
            TipoTerreno = request.TipoTerreno,
            Descripcion = request.Descripcion,
            EstaActiva = request.EstaActiva,
            CreadoEn = DateTime.UtcNow
        };

        _context.Rutas.Add(ruta);
        await _context.SaveChangesAsync();
        return ToRutaDto(ruta);
    }

    public override async Task<RutaDto> ObtenerRutaPorId(RutaIdRequest request, ServerCallContext context)
    {
        var ruta = await _context.Rutas.FindAsync(request.RutaId);
        if (ruta == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ruta no encontrada"));
        return ToRutaDto(ruta);
    }

    public override async Task<RutaDto> EditarRuta(RutaUpdateRequest request, ServerCallContext context)
    {
        var ruta = await _context.Rutas.FindAsync(request.RutaId);
        if (ruta == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ruta no encontrada"));

        if (request.HasCodigo) ruta.Codigo = request.Codigo;
        if (request.HasNombre) ruta.Nombre = request.Nombre;
        if (request.HasOrigenId) ruta.OrigenId = request.OrigenId;
        if (request.HasDestinoId) ruta.DestinoId = request.DestinoId;
        if (request.HasDistancia) ruta.Distancia = (decimal)request.Distancia;
        if (request.HasTiempoEstimado) ruta.TiempoEstimado = (decimal)request.TiempoEstimado;
        if (request.HasTipoTerreno) ruta.TipoTerreno = request.TipoTerreno;
        if (request.HasDescripcion) ruta.Descripcion = request.Descripcion;
        if (request.HasEstaActiva) ruta.EstaActiva = request.EstaActiva;

        ruta.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ToRutaDto(ruta);
    }

    public override async Task<Empty> EliminarRuta(RutaIdRequest request, ServerCallContext context)
    {
        var ruta = await _context.Rutas.FindAsync(request.RutaId);
        if (ruta == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ruta no encontrada"));

        _context.Rutas.Remove(ruta);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    public override async Task<UbicacionDto> CrearUbicacion(UbicacionCreateRequest request, ServerCallContext context)
    {
        var ubicacion = new Ubicacion
        {
            Nombre = request.Nombre,
            Direccion = request.Direccion,
            Ciudad = request.Ciudad,
            Estado = request.Estado,
            Pais = request.Pais,
            Latitud = (decimal)request.Latitud,
            Longitud = (decimal)request.Longitud,
            Tipo = request.Tipo,
            CreadoEn = DateTime.UtcNow
        };

        _context.Ubicaciones.Add(ubicacion);
        await _context.SaveChangesAsync();
        return ToUbicacionDto(ubicacion);
    }

    public override async Task<UbicacionDto> ObtenerUbicacionPorId(UbicacionIdRequest request, ServerCallContext context)
    {
        var ubicacion = await _context.Ubicaciones.FindAsync(request.UbicacionId);
        if (ubicacion == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ubicación no encontrada"));
        return ToUbicacionDto(ubicacion);
    }

    public override async Task<UbicacionDto> EditarUbicacion(UbicacionUpdateRequest request, ServerCallContext context)
    {
        var ubicacion = await _context.Ubicaciones.FindAsync(request.UbicacionId);
        if (ubicacion == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ubicación no encontrada"));

        if (request.HasNombre) ubicacion.Nombre = request.Nombre;
        if (request.HasDireccion) ubicacion.Direccion = request.Direccion;
        if (request.HasCiudad) ubicacion.Ciudad = request.Ciudad;
        if (request.HasEstado) ubicacion.Estado = request.Estado;
        if (request.HasPais) ubicacion.Pais = request.Pais;
        if (request.HasLatitud) ubicacion.Latitud = (decimal)request.Latitud;
        if (request.HasLongitud) ubicacion.Longitud = (decimal)request.Longitud;
        if (request.HasTipo) ubicacion.Tipo = request.Tipo;

        ubicacion.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ToUbicacionDto(ubicacion);
    }

    public override async Task<Empty> EliminarUbicacion(UbicacionIdRequest request, ServerCallContext context)
    {
        var ubicacion = await _context.Ubicaciones.FindAsync(request.UbicacionId);
        if (ubicacion == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ubicación no encontrada"));

        _context.Ubicaciones.Remove(ubicacion);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    public override async Task<SegmentoDto> CrearSegmento(SegmentoCreateRequest request, ServerCallContext context)
    {
        var segmento = new SegmentoRuta
        {
            RutaId = request.RutaId,
            NumeroSecuencia = request.NumeroSecuencia,
            UbicacionInicioId = request.UbicacionInicioId,
            UbicacionFinId = request.UbicacionFinId,
            DistanciaSegmento = (decimal)request.DistanciaSegmento,
            TiempoSegmento = (decimal)request.TiempoSegmento,
            TipoTerreno = request.TipoTerreno,
            Descripcion = request.Descripcion,
            CreadoEn = DateTime.UtcNow
        };

        _context.Segmentos.Add(segmento);
        await _context.SaveChangesAsync();
        return ToSegmentoDto(segmento);
    }

    public override async Task<SegmentoDto> ObtenerSegmentoPorId(SegmentoIdRequest request, ServerCallContext context)
    {
        var segmento = await _context.Segmentos.FindAsync(request.SegmentoId);
        if (segmento == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Segmento no encontrado"));
        return ToSegmentoDto(segmento);
    }

    public override async Task<SegmentoDto> EditarSegmento(SegmentoUpdateRequest request, ServerCallContext context)
    {
        var segmento = await _context.Segmentos.FindAsync(request.SegmentoId);
        if (segmento == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Segmento no encontrado"));

        if (request.HasRutaId) segmento.RutaId = request.RutaId;
        if (request.HasNumeroSecuencia) segmento.NumeroSecuencia = request.NumeroSecuencia;
        if (request.HasUbicacionInicioId) segmento.UbicacionInicioId = request.UbicacionInicioId;
        if (request.HasUbicacionFinId) segmento.UbicacionFinId = request.UbicacionFinId;
        if (request.HasDistanciaSegmento) segmento.DistanciaSegmento = (decimal)request.DistanciaSegmento;
        if (request.HasTiempoSegmento) segmento.TiempoSegmento = (decimal)request.TiempoSegmento;
        if (request.HasTipoTerreno) segmento.TipoTerreno = request.TipoTerreno;
        if (request.HasDescripcion) segmento.Descripcion = request.Descripcion;

        segmento.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return ToSegmentoDto(segmento);
    }

    public override async Task<Empty> EliminarSegmento(SegmentoIdRequest request, ServerCallContext context)
    {
        var segmento = await _context.Segmentos.FindAsync(request.SegmentoId);
        if (segmento == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Segmento no encontrado"));

        _context.Segmentos.Remove(segmento);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    public override async Task<ListaRutas> ListarRutas(Empty request, ServerCallContext context)
    {
        var list = await _context.Rutas.ToListAsync();
        var res = new ListaRutas();
        res.Rutas.AddRange(list.Select(ToRutaDto));
        return res;
    }

    public override async Task<ListaUbicaciones> ListarUbicaciones(Empty request, ServerCallContext context)
    {
        var list = await _context.Ubicaciones.ToListAsync();
        var res = new ListaUbicaciones();
        res.Ubicaciones.AddRange(list.Select(ToUbicacionDto));
        return res;
    }

    public override async Task<ListaSegmentos> ListarSegmentos(Empty request, ServerCallContext context)
    {
        var list = await _context.Segmentos.ToListAsync();
        var res = new ListaSegmentos();
        res.Segmentos.AddRange(list.Select(ToSegmentoDto));
        return res;
    }
}
