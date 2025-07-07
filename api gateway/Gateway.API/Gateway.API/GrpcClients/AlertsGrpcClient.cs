using Grpc.Net.Client;
using Gateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using AlertsGrpc = AlertsService;
using AlertsGrpcService = AlertsService.AlertsService;

using GrpcAlertDto = AlertsService.AlertaDto;

namespace Gateway.API.GrpcClients;

public class AlertsGrpcClient
{
    private readonly AlertsGrpcService.AlertsServiceClient _client;


    public AlertsGrpcClient(IConfiguration configuration)
    {
        var url = configuration["GrpcSettings:AlertsService"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("No se configuró la URL del microservicio de alertas.");

        var channel = GrpcChannel.ForAddress(url);

        _client = new AlertsGrpcService.AlertsServiceClient(channel);

    }

    public async Task<IEnumerable<AlertaDto>> GetAllAsync()
    {
        var response = await _client.ListarAlertasAsync(new Empty());
        return response.Alertas.Select(a => new AlertaDto
        {
            AlertaId = a.AlertaId,
            CodigoVehiculo = a.CodigoVehiculo,
            CodigoConductor = a.CodigoConductor,
            CodigoRuta = a.CodigoRuta,
            RegistroId = a.RegistroId,
            TipoMaquinaria = a.TipoMaquinaria,
            TipoAlerta = a.TipoAlerta,
            PorcentajeDiferencia = a.PorcentajeDiferencia,
            Estado = a.Estado,
            Descripcion = a.Descripcion,
            CreadoEn = a.CreadoEn.ToDateTime(),
            RevisadoEn = a.RevisadoEn.ToDateTime(),
            RevisadoPor = a.RevisadoPor
        });
    }

    public async Task<IEnumerable<AlertaDto>> GetByVehiculoAsync(string codigo)
    {
        var response = await _client.AlertasPorVehiculoAsync(new AlertsGrpc.CodigoVehiculoRequest { CodigoVehiculo = codigo });

        return response.Alertas.Select(MapDto);
    }

    public async Task<IEnumerable<AlertaDto>> GetByConductorAsync(string codigo)
    {
        var response = await _client.AlertasPorConductorAsync(new AlertsGrpc.CodigoConductorRequest { CodigoConductor = codigo });

        return response.Alertas.Select(MapDto);
    }

    public async Task<IEnumerable<AlertaDto>> GetByRutaAsync(string codigo)
    {
        var response = await _client.AlertasPorRutaAsync(new AlertsGrpc.CodigoRutaRequest { CodigoRuta = codigo });

        return response.Alertas.Select(MapDto);
    }

    public async Task<AlertaDto?> GetByIdAsync(int id)
    {
        try
        {
            var a = await _client.ObtenerAlertaAsync(new AlertsGrpc.AlertaIdRequest { AlertaId = id });

            return MapDto(a);
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<AlertaDto> CreateAsync(AlertaCreateRequestModel request)
    {
        var grpcRequest = new AlertsGrpc.AlertaCreateRequest
        {
            CodigoVehiculo = request.CodigoVehiculo,
            CodigoConductor = request.CodigoConductor,
            CodigoRuta = request.CodigoRuta,
            RegistroId = request.RegistroId,
            TipoMaquinaria = request.TipoMaquinaria,
            TipoAlerta = request.TipoAlerta,
            PorcentajeDiferencia = request.PorcentajeDiferencia,
            Estado = request.Estado,
            Descripcion = request.Descripcion
        };
        var a = await _client.CrearAlertaAsync(grpcRequest);
        return MapDto(a);
    }

    public async Task<AlertaDto> UpdateAsync(int id, AlertaUpdateRequestModel request)
    {
        var grpcRequest = new AlertsGrpc.AlertaUpdateRequest { AlertaId = id };

        if (request.CodigoVehiculo != null) grpcRequest.CodigoVehiculo = request.CodigoVehiculo;
        if (request.CodigoConductor != null) grpcRequest.CodigoConductor = request.CodigoConductor;
        if (request.CodigoRuta != null) grpcRequest.CodigoRuta = request.CodigoRuta;
        if (request.RegistroId.HasValue) grpcRequest.RegistroId = request.RegistroId.Value;
        if (request.TipoMaquinaria != null) grpcRequest.TipoMaquinaria = request.TipoMaquinaria;
        if (request.TipoAlerta != null) grpcRequest.TipoAlerta = request.TipoAlerta;
        if (request.PorcentajeDiferencia.HasValue) grpcRequest.PorcentajeDiferencia = request.PorcentajeDiferencia.Value;
        if (request.Estado.HasValue) grpcRequest.Estado = request.Estado.Value;
        if (request.Descripcion != null) grpcRequest.Descripcion = request.Descripcion;
        if (request.RevisadoEn.HasValue) grpcRequest.RevisadoEn = Timestamp.FromDateTime(request.RevisadoEn.Value.ToUniversalTime());
        if (request.RevisadoPor != null) grpcRequest.RevisadoPor = request.RevisadoPor;
        var a = await _client.EditarAlertaAsync(grpcRequest);
        return MapDto(a);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _client.EliminarAlertaAsync(new AlertsGrpc.AlertaIdRequest { AlertaId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
    }

    private static AlertaDto MapDto(GrpcAlertDto a) => new AlertaDto
    {
        AlertaId = a.AlertaId,
        CodigoVehiculo = a.CodigoVehiculo,
        CodigoConductor = a.CodigoConductor,
        CodigoRuta = a.CodigoRuta,
        RegistroId = a.RegistroId,
        TipoMaquinaria = a.TipoMaquinaria,
        TipoAlerta = a.TipoAlerta,
        PorcentajeDiferencia = a.PorcentajeDiferencia,
        Estado = a.Estado,
        Descripcion = a.Descripcion,
        CreadoEn = a.CreadoEn.ToDateTime(),
        RevisadoEn = a.RevisadoEn.ToDateTime(),
        RevisadoPor = a.RevisadoPor
    };
}
