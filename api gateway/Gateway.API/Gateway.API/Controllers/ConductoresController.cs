using Microsoft.AspNetCore.Mvc;
using Gateway.API.GrpcClients;
using Gateway.API.Models;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConductoresController : ControllerBase
{
    private readonly DriverGrpcClient _grpcClient;

    public ConductoresController(DriverGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var items = await _grpcClient.GetAllAsync();
            return Ok(items);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var item = await _grpcClient.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = $"Conductor con ID {id} no encontrado." });
            return Ok(item);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ConductorCreateRequest request)
    {
        try
        {
            var created = await _grpcClient.CreateAsync(request);
            return Ok(created);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ConductorUpdateRequest request)
    {
        try
        {
            var updated = await _grpcClient.UpdateAsync(id, request);
            return Ok(updated);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _grpcClient.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Conductor con ID {id} no encontrado." });
            return NoContent();
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
