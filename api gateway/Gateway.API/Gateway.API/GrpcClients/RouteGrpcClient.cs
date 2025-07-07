using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using RoutesService;
using ProtoRutaDto = RoutesService.RutaDto;
using ProtoUbicacionDto = RoutesService.UbicacionDto;
using ProtoSegmentoDto = RoutesService.SegmentoDto;

namespace Gateway.API.GrpcClients;

public class RouteGrpcClient
{
    private readonly RouteService.RouteServiceClient _client;

    public RouteGrpcClient(IConfiguration configuration)
    {
        var url = configuration["GrpcSettings:RoutesService"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("No se configur\u00f3 la URL del microservicio de rutas.");

        var channel = GrpcChannel.ForAddress(url);
        _client = new RouteService.RouteServiceClient(channel);
    }

    public async Task<IEnumerable<Gateway.API.Models.RutaDto>> ListarRutasAsync()
    {
        var response = await _client.ListarRutasAsync(new Empty());
        return response.Rutas.Select(r => new Gateway.API.Models.RutaDto
        {
            RutaId = r.RutaId,
            Codigo = r.Codigo,
            Nombre = r.Nombre,
            OrigenId = r.OrigenId,
            DestinoId = r.DestinoId,
            Distancia = r.Distancia,
            TiempoEstimado = r.TiempoEstimado,
            TipoTerreno = r.TipoTerreno,
            Descripcion = r.Descripcion,
            EstaActiva = r.EstaActiva
        });
    }

    public async Task<Gateway.API.Models.RutaDto?> ObtenerRutaAsync(int id)
    {
        try
        {
            var r = await _client.ObtenerRutaPorIdAsync(new RutaIdRequest { RutaId = id });
            return new Gateway.API.Models.RutaDto
            {
                RutaId = r.RutaId,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                OrigenId = r.OrigenId,
                DestinoId = r.DestinoId,
                Distancia = r.Distancia,
                TiempoEstimado = r.TiempoEstimado,
                TipoTerreno = r.TipoTerreno,
                Descripcion = r.Descripcion,
                EstaActiva = r.EstaActiva
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Gateway.API.Models.RutaDto> CrearRutaAsync(Gateway.API.Models.RutaCreateRequest request)
    {
        var grpc = new RutaCreateRequest
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            OrigenId = request.OrigenId,
            DestinoId = request.DestinoId,
            Distancia = request.Distancia,
            TiempoEstimado = request.TiempoEstimado,
            TipoTerreno = request.TipoTerreno ?? string.Empty,
            Descripcion = request.Descripcion ?? string.Empty,
            EstaActiva = request.EstaActiva
        };

        var r = await _client.CrearRutaAsync(grpc);
        return new Gateway.API.Models.RutaDto
        {
            RutaId = r.RutaId,
            Codigo = r.Codigo,
            Nombre = r.Nombre,
            OrigenId = r.OrigenId,
            DestinoId = r.DestinoId,
            Distancia = r.Distancia,
            TiempoEstimado = r.TiempoEstimado,
            TipoTerreno = r.TipoTerreno,
            Descripcion = r.Descripcion,
            EstaActiva = r.EstaActiva
        };
    }

    public async Task<Gateway.API.Models.RutaDto?> EditarRutaAsync(int id, Gateway.API.Models.RutaUpdateRequest request)
    {
        var grpc = new RutaUpdateRequest { RutaId = id };

        if (request.Codigo != null) grpc.Codigo = request.Codigo;
        if (request.Nombre != null) grpc.Nombre = request.Nombre;
        if (request.OrigenId.HasValue) grpc.OrigenId = request.OrigenId.Value;
        if (request.DestinoId.HasValue) grpc.DestinoId = request.DestinoId.Value;
        if (request.Distancia.HasValue) grpc.Distancia = request.Distancia.Value;
        if (request.TiempoEstimado.HasValue) grpc.TiempoEstimado = request.TiempoEstimado.Value;
        if (request.TipoTerreno != null) grpc.TipoTerreno = request.TipoTerreno;
        if (request.Descripcion != null) grpc.Descripcion = request.Descripcion;
        if (request.EstaActiva.HasValue) grpc.EstaActiva = request.EstaActiva.Value;

        try
        {
            var r = await _client.EditarRutaAsync(grpc);
            return new Gateway.API.Models.RutaDto
            {
                RutaId = r.RutaId,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                OrigenId = r.OrigenId,
                DestinoId = r.DestinoId,
                Distancia = r.Distancia,
                TiempoEstimado = r.TiempoEstimado,
                TipoTerreno = r.TipoTerreno,
                Descripcion = r.Descripcion,
                EstaActiva = r.EstaActiva
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> EliminarRutaAsync(int id)
    {
        try
        {
            await _client.EliminarRutaAsync(new RutaIdRequest { RutaId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Gateway.API.Models.UbicacionDto>> ListarUbicacionesAsync()
    {
        var response = await _client.ListarUbicacionesAsync(new Empty());
        return response.Ubicaciones.Select(u => new Gateway.API.Models.UbicacionDto
        {
            UbicacionId = u.UbicacionId,
            Nombre = u.Nombre,
            Direccion = u.Direccion,
            Ciudad = u.Ciudad,
            Estado = u.Estado,
            Pais = u.Pais,
            Latitud = u.Latitud,
            Longitud = u.Longitud,
            Tipo = u.Tipo
        });
    }

    public async Task<Gateway.API.Models.UbicacionDto?> ObtenerUbicacionAsync(int id)
    {
        try
        {
            var u = await _client.ObtenerUbicacionPorIdAsync(new UbicacionIdRequest { UbicacionId = id });
            return new Gateway.API.Models.UbicacionDto
            {
                UbicacionId = u.UbicacionId,
                Nombre = u.Nombre,
                Direccion = u.Direccion,
                Ciudad = u.Ciudad,
                Estado = u.Estado,
                Pais = u.Pais,
                Latitud = u.Latitud,
                Longitud = u.Longitud,
                Tipo = u.Tipo
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Gateway.API.Models.UbicacionDto> CrearUbicacionAsync(Gateway.API.Models.UbicacionCreateRequest request)
    {
        var grpc = new UbicacionCreateRequest
        {
            Nombre = request.Nombre,
            Direccion = request.Direccion ?? string.Empty,
            Ciudad = request.Ciudad ?? string.Empty,
            Estado = request.Estado ?? string.Empty,
            Pais = request.Pais ?? string.Empty,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            Tipo = request.Tipo ?? string.Empty
        };

        var u = await _client.CrearUbicacionAsync(grpc);
        return new Gateway.API.Models.UbicacionDto
        {
            UbicacionId = u.UbicacionId,
            Nombre = u.Nombre,
            Direccion = u.Direccion,
            Ciudad = u.Ciudad,
            Estado = u.Estado,
            Pais = u.Pais,
            Latitud = u.Latitud,
            Longitud = u.Longitud,
            Tipo = u.Tipo
        };
    }

    public async Task<Gateway.API.Models.UbicacionDto?> EditarUbicacionAsync(int id, Gateway.API.Models.UbicacionUpdateRequest request)
    {
        var grpc = new UbicacionUpdateRequest { UbicacionId = id };

        if (request.Nombre != null) grpc.Nombre = request.Nombre;
        if (request.Direccion != null) grpc.Direccion = request.Direccion;
        if (request.Ciudad != null) grpc.Ciudad = request.Ciudad;
        if (request.Estado != null) grpc.Estado = request.Estado;
        if (request.Pais != null) grpc.Pais = request.Pais;
        if (request.Latitud.HasValue) grpc.Latitud = request.Latitud.Value;
        if (request.Longitud.HasValue) grpc.Longitud = request.Longitud.Value;
        if (request.Tipo != null) grpc.Tipo = request.Tipo;

        try
        {
            var u = await _client.EditarUbicacionAsync(grpc);
            return new Gateway.API.Models.UbicacionDto
            {
                UbicacionId = u.UbicacionId,
                Nombre = u.Nombre,
                Direccion = u.Direccion,
                Ciudad = u.Ciudad,
                Estado = u.Estado,
                Pais = u.Pais,
                Latitud = u.Latitud,
                Longitud = u.Longitud,
                Tipo = u.Tipo
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> EliminarUbicacionAsync(int id)
    {
        try
        {
            await _client.EliminarUbicacionAsync(new UbicacionIdRequest { UbicacionId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Gateway.API.Models.SegmentoDto>> ListarSegmentosAsync()
    {
        var response = await _client.ListarSegmentosAsync(new Empty());
        return response.Segmentos.Select(s => new Gateway.API.Models.SegmentoDto
        {
            SegmentoId = s.SegmentoId,
            RutaId = s.RutaId,
            NumeroSecuencia = s.NumeroSecuencia,
            UbicacionInicioId = s.UbicacionInicioId,
            UbicacionFinId = s.UbicacionFinId,
            DistanciaSegmento = s.DistanciaSegmento,
            TiempoSegmento = s.TiempoSegmento,
            TipoTerreno = s.TipoTerreno,
            Descripcion = s.Descripcion
        });
    }

    public async Task<Gateway.API.Models.SegmentoDto?> ObtenerSegmentoAsync(int id)
    {
        try
        {
            var s = await _client.ObtenerSegmentoPorIdAsync(new SegmentoIdRequest { SegmentoId = id });
            return new Gateway.API.Models.SegmentoDto
            {
                SegmentoId = s.SegmentoId,
                RutaId = s.RutaId,
                NumeroSecuencia = s.NumeroSecuencia,
                UbicacionInicioId = s.UbicacionInicioId,
                UbicacionFinId = s.UbicacionFinId,
                DistanciaSegmento = s.DistanciaSegmento,
                TiempoSegmento = s.TiempoSegmento,
                TipoTerreno = s.TipoTerreno,
                Descripcion = s.Descripcion
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Gateway.API.Models.SegmentoDto> CrearSegmentoAsync(Gateway.API.Models.SegmentoCreateRequest request)
    {
        var grpc = new SegmentoCreateRequest
        {
            RutaId = request.RutaId,
            NumeroSecuencia = request.NumeroSecuencia,
            UbicacionInicioId = request.UbicacionInicioId,
            UbicacionFinId = request.UbicacionFinId,
            DistanciaSegmento = request.DistanciaSegmento,
            TiempoSegmento = request.TiempoSegmento,
            TipoTerreno = request.TipoTerreno ?? string.Empty,
            Descripcion = request.Descripcion ?? string.Empty
        };

        var s = await _client.CrearSegmentoAsync(grpc);
        return new Gateway.API.Models.SegmentoDto
        {
            SegmentoId = s.SegmentoId,
            RutaId = s.RutaId,
            NumeroSecuencia = s.NumeroSecuencia,
            UbicacionInicioId = s.UbicacionInicioId,
            UbicacionFinId = s.UbicacionFinId,
            DistanciaSegmento = s.DistanciaSegmento,
            TiempoSegmento = s.TiempoSegmento,
            TipoTerreno = s.TipoTerreno,
            Descripcion = s.Descripcion
        };
    }

    public async Task<Gateway.API.Models.SegmentoDto?> EditarSegmentoAsync(int id, Gateway.API.Models.SegmentoUpdateRequest request)
    {
        var grpc = new SegmentoUpdateRequest { SegmentoId = id };

        if (request.RutaId.HasValue) grpc.RutaId = request.RutaId.Value;
        if (request.NumeroSecuencia.HasValue) grpc.NumeroSecuencia = request.NumeroSecuencia.Value;
        if (request.UbicacionInicioId.HasValue) grpc.UbicacionInicioId = request.UbicacionInicioId.Value;
        if (request.UbicacionFinId.HasValue) grpc.UbicacionFinId = request.UbicacionFinId.Value;
        if (request.DistanciaSegmento.HasValue) grpc.DistanciaSegmento = request.DistanciaSegmento.Value;
        if (request.TiempoSegmento.HasValue) grpc.TiempoSegmento = request.TiempoSegmento.Value;
        if (request.TipoTerreno != null) grpc.TipoTerreno = request.TipoTerreno;
        if (request.Descripcion != null) grpc.Descripcion = request.Descripcion;

        try
        {
            var s = await _client.EditarSegmentoAsync(grpc);
            return new Gateway.API.Models.SegmentoDto
            {
                SegmentoId = s.SegmentoId,
                RutaId = s.RutaId,
                NumeroSecuencia = s.NumeroSecuencia,
                UbicacionInicioId = s.UbicacionInicioId,
                UbicacionFinId = s.UbicacionFinId,
                DistanciaSegmento = s.DistanciaSegmento,
                TiempoSegmento = s.TiempoSegmento,
                TipoTerreno = s.TipoTerreno,
                Descripcion = s.Descripcion
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> EliminarSegmentoAsync(int id)
    {
        try
        {
            await _client.EliminarSegmentoAsync(new SegmentoIdRequest { SegmentoId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
    }
}
