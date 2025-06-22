using Grpc.Net.Client;
using Gateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using VehicleService;
using VehicleGrpc = VehicleService.VehiculoDto; 

namespace Gateway.API.GrpcClients;

public class VehiculoGrpcClient
{
    private readonly VehiculoService.VehiculoServiceClient _client;

    public VehiculoGrpcClient(IConfiguration configuration)
    {
        var url = configuration["GrpcSettings:VehiclesService"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("No se configuró la URL del microservicio de vehículos.");

        var channel = GrpcChannel.ForAddress(url);
        _client = new VehiculoService.VehiculoServiceClient(channel);
    }

    public async Task<IEnumerable<Gateway.API.Models.VehiculoDto>> GetAllAsync()
    {
        try
        {
            var response = await _client.ListarVehiculosAsync(new Empty());

            return response.Vehiculos.Select((VehicleGrpc v) => new Gateway.API.Models.VehiculoDto
            {
                Id = v.Id,
                Placa = v.Placa,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                TipoMaquinariaId = v.TipoMaquinariaId,
                EstadoOperativo = v.EstadoOperativo,
                CapacidadTanqueGalones = v.CapacidadTanqueGalones,
                CombustibleActualGalones = v.CombustibleActualGalones
            });
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new ApplicationException($"Error desde el microservicio de vehículos: {ex.Status.Detail}", ex);
        }
    }

    public async Task<Gateway.API.Models.VehiculoDto?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _client.ObtenerVehiculoPorIdAsync(new VehiculoIdRequest { Id = id });

            return new Gateway.API.Models.VehiculoDto
            {
                Id = response.Id,
                Placa = response.Placa,
                Marca = response.Marca,
                Modelo = response.Modelo,
                Anio = response.Anio,
                TipoMaquinariaId = response.TipoMaquinariaId,
                EstadoOperativo = response.EstadoOperativo,
                CapacidadTanqueGalones = response.CapacidadTanqueGalones,
                CombustibleActualGalones = response.CombustibleActualGalones
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al obtener vehículo por ID: {ex.Message}", ex);
        }
    }


    public async Task<Gateway.API.Models.VehiculoDto> CreateAsync(VehiculoCreateRequest request)
    {
        try
        {
            var grpcRequest = new VehiculoRequest
            {
                Placa = request.Placa,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Anio = request.Anio,
                TipoMaquinariaId = request.TipoMaquinariaId,
                CapacidadTanqueGalones = request.CapacidadTanqueGalones,
                CombustibleActualGalones = request.CombustibleActualGalones
            };

            var response = await _client.CrearVehiculoAsync(grpcRequest);

            return new Gateway.API.Models.VehiculoDto
            {
                Id = response.Id,
                Placa = response.Placa,
                Marca = response.Marca,
                Modelo = response.Modelo,
                Anio = response.Anio,
                TipoMaquinariaId = response.TipoMaquinariaId,
                EstadoOperativo = response.EstadoOperativo,
                CapacidadTanqueGalones = response.CapacidadTanqueGalones,
                CombustibleActualGalones = response.CombustibleActualGalones
            };
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new ApplicationException($"Error al crear vehículo: {ex.Status.Detail}", ex);
        }
    }


    public async Task<Gateway.API.Models.VehiculoDto> UpdateAsync(int id, VehiculoUpdateRequest request)
    {
        try
        {
            var grpcRequest = new EditarVehiculoRequest
            {
                Id = id,
            };

            if (request.Placa != null) grpcRequest.Placa = request.Placa;
            if (request.Marca != null) grpcRequest.Marca = request.Marca;
            if (request.Modelo != null) grpcRequest.Modelo = request.Modelo;
            if (request.Anio.HasValue) grpcRequest.Anio = request.Anio.Value;
            if (request.TipoMaquinariaId.HasValue) grpcRequest.TipoMaquinariaId = request.TipoMaquinariaId.Value;
            if (request.EstadoOperativo.HasValue) grpcRequest.EstadoOperativo = request.EstadoOperativo.Value;
            if (request.CapacidadTanqueGalones.HasValue) grpcRequest.CapacidadTanqueGalones = request.CapacidadTanqueGalones.Value;
            if (request.CombustibleActualGalones.HasValue) grpcRequest.CombustibleActualGalones = request.CombustibleActualGalones.Value;

            var response = await _client.EditarVehiculoAsync(grpcRequest);

            return new Gateway.API.Models.VehiculoDto
            {
                Id = response.Id,
                Placa = response.Placa,
                Marca = response.Marca,
                Modelo = response.Modelo,
                Anio = response.Anio,
                TipoMaquinariaId = response.TipoMaquinariaId,
                EstadoOperativo = response.EstadoOperativo,
                CapacidadTanqueGalones = response.CapacidadTanqueGalones,
                CombustibleActualGalones = response.CombustibleActualGalones
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al actualizar vehículo: {ex.Message}", ex);
        }
    }


    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _client.EliminarVehiculoAsync(new VehiculoIdRequest { Id = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al eliminar vehículo: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Gateway.API.Models.VehiculoDto>> FiltrarAsync(VehiculoFiltroRequest request)
    {
        try
        {
            var grpcRequest = new FiltroVehiculoRequest();

            if (request.Placa != null) grpcRequest.Placa = request.Placa;
            if (request.Marca != null) grpcRequest.Marca = request.Marca;
            if (request.Modelo != null) grpcRequest.Modelo = request.Modelo;
            if (request.Anio.HasValue) grpcRequest.Anio = request.Anio.Value;
            if (request.TipoMaquinariaId.HasValue) grpcRequest.TipoMaquinariaId = request.TipoMaquinariaId.Value;
            if (request.EstadoOperativo.HasValue) grpcRequest.EstadoOperativo = request.EstadoOperativo.Value;
            if (request.CapacidadMinima.HasValue) grpcRequest.CapacidadMinima = request.CapacidadMinima.Value;
            if (request.CombustibleMinimo.HasValue) grpcRequest.CombustibleMinimo = request.CombustibleMinimo.Value;

            var response = await _client.FiltrarVehiculosAsync(grpcRequest);

            return response.Vehiculos.Select(v => new Gateway.API.Models.VehiculoDto
            {
                Id = v.Id,
                Placa = v.Placa,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                TipoMaquinariaId = v.TipoMaquinariaId,
                EstadoOperativo = v.EstadoOperativo,
                CapacidadTanqueGalones = v.CapacidadTanqueGalones,
                CombustibleActualGalones = v.CombustibleActualGalones
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al filtrar vehículos: {ex.Message}", ex);
        }
    }


}
