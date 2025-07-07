using Gateway.API.GrpcClients;
using Gateway.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiposMaquinariaController : ControllerBase
{
    private readonly FuelGrpcClient _grpc;

    public TiposMaquinariaController(FuelGrpcClient grpc)
    {
        _grpc = grpc;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _grpc.GetTiposAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _grpc.GetTipoByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TipoMaquinariaCreateRequest request)
    {
        var created = await _grpc.CreateTipoAsync(request);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TipoMaquinariaUpdateRequest request)
    {
        var updated = await _grpc.UpdateTipoAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _grpc.DeleteTipoAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
