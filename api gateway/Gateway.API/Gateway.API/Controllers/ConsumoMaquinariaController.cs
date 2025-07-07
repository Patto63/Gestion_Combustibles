using Gateway.API.GrpcClients;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/consumo-maquinaria")]
public class ConsumoMaquinariaController : ControllerBase
{
    private readonly FuelGrpcClient _grpcClient;

    public ConsumoMaquinariaController(FuelGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _grpcClient.GetConsumoTipoMaquinariaAsync();
        return Ok(items);
    }
}
