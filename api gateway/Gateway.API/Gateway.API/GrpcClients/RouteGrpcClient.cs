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
}
