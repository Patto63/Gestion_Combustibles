using Gateway.API.GrpcClients;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RutasController : ControllerBase
{
    private readonly RouteGrpcClient _grpcClient;

    public RutasController(RouteGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rutas = await _grpcClient.ListarRutasAsync();
        return Ok(rutas);
    }

    [HttpGet("ubicaciones")]
    public async Task<IActionResult> GetUbicaciones()
    {
        var ubicaciones = await _grpcClient.ListarUbicacionesAsync();
        return Ok(ubicaciones);
    }

    [HttpGet("segmentos")]
    public async Task<IActionResult> GetSegmentos()
    {
        var segmentos = await _grpcClient.ListarSegmentosAsync();
        return Ok(segmentos);
    }
}
