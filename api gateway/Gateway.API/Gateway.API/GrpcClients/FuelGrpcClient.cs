using Grpc.Net.Client;
using Gateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using FuelProto = FuelService;
using FuelRecordGrpc = FuelService.RegistroCombustibleDto;
using FuelGrpcService = FuelService.FuelService;

namespace Gateway.API.GrpcClients;

public class FuelGrpcClient
{
    private readonly FuelGrpcService.FuelServiceClient _client;

    public FuelGrpcClient(IConfiguration configuration)
    {
        var url = configuration["GrpcSettings:FuelService"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("No se configuró la URL del microservicio de combustible.");
        var channel = GrpcChannel.ForAddress(url);
        _client = new FuelGrpcService.FuelServiceClient(channel);
    }

    public async Task<IEnumerable<Models.RegistroCombustibleDto>> GetRegistrosAsync()
    {
        var response = await _client.ListarRegistrosAsync(new Empty());
        return response.Registros.Select(r => new Models.RegistroCombustibleDto
        {
            RegistroId = r.RegistroId,
            Fecha = r.Fecha.ToDateTime(),
            CodigoVehiculo = r.CodigoVehiculo,
            CodigoConductor = r.CodigoConductor,
            CodigoRuta = r.CodigoRuta,
            TipoMaquinaria = r.TipoMaquinaria,
            CantidadCombustible = r.CantidadCombustible,
            PrecioCombustible = r.PrecioCombustible,
            CostoTotal = r.CostoTotal
        });
    }

    public async Task<IEnumerable<TipoMaquinariaDto>> GetTiposAsync()
    {
        var response = await _client.ListarTiposMaquinariaAsync(new Empty());
        return response.Tipos.Select(t => new TipoMaquinariaDto
        {
            TipoMaquinariaId = t.TipoMaquinariaId,
            Nombre = t.Nombre
        });
    }

    public async Task<TipoMaquinariaDto?> GetTipoByIdAsync(int id)
    {
        try
        {
            var t = await _client.ObtenerTipoMaquinariaAsync(new FuelProto.TipoMaquinariaIdRequest { TipoMaquinariaId = id });
            return new TipoMaquinariaDto { TipoMaquinariaId = t.TipoMaquinariaId, Nombre = t.Nombre };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<TipoMaquinariaDto> CreateTipoAsync(TipoMaquinariaCreateRequest request)
    {
        var res = await _client.CrearTipoMaquinariaAsync(new FuelProto.TipoMaquinariaCreateRequest { Nombre = request.Nombre });
        return new TipoMaquinariaDto { TipoMaquinariaId = res.TipoMaquinariaId, Nombre = res.Nombre };
    }

    public async Task<TipoMaquinariaDto> UpdateTipoAsync(int id, TipoMaquinariaUpdateRequest request)
    {
        var grpc = new FuelProto.TipoMaquinariaUpdateRequest { TipoMaquinariaId = id };
        if (request.Nombre != null) grpc.Nombre = request.Nombre;
        var res = await _client.EditarTipoMaquinariaAsync(grpc);
        return new TipoMaquinariaDto { TipoMaquinariaId = res.TipoMaquinariaId, Nombre = res.Nombre };
    }

    public async Task<bool> DeleteTipoAsync(int id)
    {
        try
        {
            await _client.EliminarTipoMaquinariaAsync(new FuelProto.TipoMaquinariaIdRequest { TipoMaquinariaId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
    }
}
