using Microsoft.AspNetCore.Mvc;
using Gateway.API.GrpcClients;
using Gateway.API.Models;

namespace Gateway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly VehiculoGrpcClient _grpcClient;

    public VehiculosController(VehiculoGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var vehiculos = await _grpcClient.GetAllAsync();
            return Ok(vehiculos);
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
            var vehiculo = await _grpcClient.GetByIdAsync(id);
            if (vehiculo == null)
                return NotFound(new { message = $"Vehículo con ID {id} no encontrado." });

            return Ok(vehiculo);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehiculoCreateRequest request)
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
    public async Task<IActionResult> Update(int id, [FromBody] VehiculoUpdateRequest request)
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
            var eliminado = await _grpcClient.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { message = $"Vehículo con ID {id} no encontrado para eliminar." });

            return NoContent(); // 204
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("filtrar")]
    public async Task<IActionResult> Filtrar([FromBody] VehiculoFiltroRequest request)
    {
        try
        {
            var resultado = await _grpcClient.FiltrarAsync(request);
            return Ok(resultado);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }



}
