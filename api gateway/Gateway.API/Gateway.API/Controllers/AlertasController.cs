using Microsoft.AspNetCore.Mvc;
using Gateway.API.GrpcClients;
using Gateway.API.Models;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertasController : ControllerBase
{
    private readonly AlertsGrpcClient _grpcClient;

    public AlertasController(AlertsGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _grpcClient.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var alerta = await _grpcClient.GetByIdAsync(id);
        if (alerta == null)
            return NotFound();
        return Ok(alerta);
    }

    [HttpGet("vehiculo/{codigo}")]
    public async Task<IActionResult> PorVehiculo(string codigo)
    {
        var items = await _grpcClient.GetByVehiculoAsync(codigo);
        return Ok(items);
    }

    [HttpGet("conductor/{codigo}")]
    public async Task<IActionResult> PorConductor(string codigo)
    {
        var items = await _grpcClient.GetByConductorAsync(codigo);
        return Ok(items);
    }

    [HttpGet("ruta/{codigo}")]
    public async Task<IActionResult> PorRuta(string codigo)
    {
        var items = await _grpcClient.GetByRutaAsync(codigo);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlertaCreateRequestModel request)
    {
        var created = await _grpcClient.CreateAsync(request);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AlertaUpdateRequestModel request)
    {
        var updated = await _grpcClient.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _grpcClient.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
