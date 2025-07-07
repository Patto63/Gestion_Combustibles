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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ruta = await _grpcClient.ObtenerRutaAsync(id);
        if (ruta == null) return NotFound();
        return Ok(ruta);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Gateway.API.Models.RutaCreateRequest request)
    {
        var created = await _grpcClient.CrearRutaAsync(request);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Gateway.API.Models.RutaUpdateRequest request)
    {
        var updated = await _grpcClient.EditarRutaAsync(id, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _grpcClient.EliminarRutaAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("ubicaciones")]
    public async Task<IActionResult> GetUbicaciones()
    {
        var ubicaciones = await _grpcClient.ListarUbicacionesAsync();
        return Ok(ubicaciones);
    }

    [HttpGet("ubicaciones/{id}")]
    public async Task<IActionResult> GetUbicacionById(int id)
    {
        var ubicacion = await _grpcClient.ObtenerUbicacionAsync(id);
        if (ubicacion == null) return NotFound();
        return Ok(ubicacion);
    }

    [HttpPost("ubicaciones")]
    public async Task<IActionResult> CreateUbicacion([FromBody] Gateway.API.Models.UbicacionCreateRequest request)
    {
        var created = await _grpcClient.CrearUbicacionAsync(request);
        return Ok(created);
    }

    [HttpPut("ubicaciones/{id}")]
    public async Task<IActionResult> UpdateUbicacion(int id, [FromBody] Gateway.API.Models.UbicacionUpdateRequest request)
    {
        var updated = await _grpcClient.EditarUbicacionAsync(id, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("ubicaciones/{id}")]
    public async Task<IActionResult> DeleteUbicacion(int id)
    {
        var ok = await _grpcClient.EliminarUbicacionAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("segmentos")]
    public async Task<IActionResult> GetSegmentos()
    {
        var segmentos = await _grpcClient.ListarSegmentosAsync();
        return Ok(segmentos);
    }

    [HttpGet("segmentos/{id}")]
    public async Task<IActionResult> GetSegmentoById(int id)
    {
        var segmento = await _grpcClient.ObtenerSegmentoAsync(id);
        if (segmento == null) return NotFound();
        return Ok(segmento);
    }

    [HttpPost("segmentos")]
    public async Task<IActionResult> CreateSegmento([FromBody] Gateway.API.Models.SegmentoCreateRequest request)
    {
        var created = await _grpcClient.CrearSegmentoAsync(request);
        return Ok(created);
    }

    [HttpPut("segmentos/{id}")]
    public async Task<IActionResult> UpdateSegmento(int id, [FromBody] Gateway.API.Models.SegmentoUpdateRequest request)
    {
        var updated = await _grpcClient.EditarSegmentoAsync(id, request);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("segmentos/{id}")]
    public async Task<IActionResult> DeleteSegmento(int id)
    {
        var ok = await _grpcClient.EliminarSegmentoAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
