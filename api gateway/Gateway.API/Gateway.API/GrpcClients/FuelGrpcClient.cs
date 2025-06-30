using Grpc.Net.Client;
using Gateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using FuelService;
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
}
