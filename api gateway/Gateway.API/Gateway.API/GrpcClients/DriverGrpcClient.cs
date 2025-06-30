using Grpc.Net.Client;
using Gateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using DriversService;
using DriverGrpc = DriversService.ConductorDto;
using DriverCreateGrpc = DriversService.ConductorCreateRequest;
using DriverUpdateGrpc = DriversService.ConductorUpdateRequest;

namespace Gateway.API.GrpcClients;

public class DriverGrpcClient
{
    private readonly DriverService.DriverServiceClient _client;

    public DriverGrpcClient(IConfiguration configuration)
    {
        var url = configuration["GrpcSettings:DriversService"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("No se configur\u00f3 la URL del microservicio de conductores.");

        var channel = GrpcChannel.ForAddress(url);
        _client = new DriverService.DriverServiceClient(channel);
    }

    public async Task<IEnumerable<Models.ConductorDto>> GetAllAsync()
    {
        try
        {
            var response = await _client.ListarConductoresAsync(new Empty());
            return response.Conductores.Select(c => new Models.ConductorDto
            {
                ConductorId = c.ConductorId,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NumeroDocumento = c.NumeroDocumento,
                NumeroLicencia = c.NumeroLicencia,
                TipoLicencia = c.TipoLicencia,
                FechaExpiracionLicencia = c.FechaExpiracionLicencia.ToDateTime(),
                FechaNacimiento = c.FechaNacimiento.ToDateTime(),
                NumeroTelefono = c.NumeroTelefono,
                CorreoElectronico = c.CorreoElectronico,
                Direccion = c.Direccion,
                FechaIngreso = c.FechaIngreso.ToDateTime(),
                Estado = c.Estado
            });
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new ApplicationException($"Error desde el microservicio de conductores: {ex.Status.Detail}", ex);
        }
    }

    public async Task<Models.ConductorDto?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _client.ObtenerConductorPorIdAsync(new ConductorIdRequest { ConductorId = id });
            return new Models.ConductorDto
            {
                ConductorId = response.ConductorId,
                Codigo = response.Codigo,
                Nombre = response.Nombre,
                Apellido = response.Apellido,
                NumeroDocumento = response.NumeroDocumento,
                NumeroLicencia = response.NumeroLicencia,
                TipoLicencia = response.TipoLicencia,
                FechaExpiracionLicencia = response.FechaExpiracionLicencia.ToDateTime(),
                FechaNacimiento = response.FechaNacimiento.ToDateTime(),
                NumeroTelefono = response.NumeroTelefono,
                CorreoElectronico = response.CorreoElectronico,
                Direccion = response.Direccion,
                FechaIngreso = response.FechaIngreso.ToDateTime(),
                Estado = response.Estado
            };
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al obtener conductor por ID: {ex.Message}", ex);
        }
    }

    public async Task<Models.ConductorDto> CreateAsync(Models.ConductorCreateRequest request)
    {
        try
        {
            var grpcRequest = new DriverCreateGrpc
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                NumeroDocumento = request.NumeroDocumento,
                NumeroLicencia = request.NumeroLicencia,
                TipoLicencia = request.TipoLicencia,
                FechaExpiracionLicencia = Timestamp.FromDateTime(request.FechaExpiracionLicencia.ToUniversalTime()),
                FechaNacimiento = Timestamp.FromDateTime(request.FechaNacimiento.ToUniversalTime()),
                NumeroTelefono = request.NumeroTelefono ?? string.Empty,
                CorreoElectronico = request.CorreoElectronico ?? string.Empty,
                Direccion = request.Direccion ?? string.Empty,
                FechaIngreso = Timestamp.FromDateTime(request.FechaIngreso.ToUniversalTime())
            };

            var response = await _client.CrearConductorAsync(grpcRequest);

            return new Models.ConductorDto
            {
                ConductorId = response.ConductorId,
                Codigo = response.Codigo,
                Nombre = response.Nombre,
                Apellido = response.Apellido,
                NumeroDocumento = response.NumeroDocumento,
                NumeroLicencia = response.NumeroLicencia,
                TipoLicencia = response.TipoLicencia,
                FechaExpiracionLicencia = response.FechaExpiracionLicencia.ToDateTime(),
                FechaNacimiento = response.FechaNacimiento.ToDateTime(),
                NumeroTelefono = response.NumeroTelefono,
                CorreoElectronico = response.CorreoElectronico,
                Direccion = response.Direccion,
                FechaIngreso = response.FechaIngreso.ToDateTime(),
                Estado = response.Estado
            };
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new ApplicationException($"Error al crear conductor: {ex.Status.Detail}", ex);
        }
    }

    public async Task<Models.ConductorDto> UpdateAsync(int id, Models.ConductorUpdateRequest request)
    {
        try
        {
            var grpcRequest = new DriverUpdateGrpc
            {
                ConductorId = id
            };

            if (request.Codigo != null) grpcRequest.Codigo = request.Codigo;
            if (request.Nombre != null) grpcRequest.Nombre = request.Nombre;
            if (request.Apellido != null) grpcRequest.Apellido = request.Apellido;
            if (request.NumeroDocumento != null) grpcRequest.NumeroDocumento = request.NumeroDocumento;
            if (request.NumeroLicencia != null) grpcRequest.NumeroLicencia = request.NumeroLicencia;
            if (request.TipoLicencia != null) grpcRequest.TipoLicencia = request.TipoLicencia;
            if (request.FechaExpiracionLicencia.HasValue) grpcRequest.FechaExpiracionLicencia = Timestamp.FromDateTime(request.FechaExpiracionLicencia.Value.ToUniversalTime());
            if (request.FechaNacimiento.HasValue) grpcRequest.FechaNacimiento = Timestamp.FromDateTime(request.FechaNacimiento.Value.ToUniversalTime());
            if (request.NumeroTelefono != null) grpcRequest.NumeroTelefono = request.NumeroTelefono;
            if (request.CorreoElectronico != null) grpcRequest.CorreoElectronico = request.CorreoElectronico;
            if (request.Direccion != null) grpcRequest.Direccion = request.Direccion;
            if (request.Estado.HasValue) grpcRequest.Estado = request.Estado.Value;

            var response = await _client.EditarConductorAsync(grpcRequest);

            return new Models.ConductorDto
            {
                ConductorId = response.ConductorId,
                Codigo = response.Codigo,
                Nombre = response.Nombre,
                Apellido = response.Apellido,
                NumeroDocumento = response.NumeroDocumento,
                NumeroLicencia = response.NumeroLicencia,
                TipoLicencia = response.TipoLicencia,
                FechaExpiracionLicencia = response.FechaExpiracionLicencia.ToDateTime(),
                FechaNacimiento = response.FechaNacimiento.ToDateTime(),
                NumeroTelefono = response.NumeroTelefono,
                CorreoElectronico = response.CorreoElectronico,
                Direccion = response.Direccion,
                FechaIngreso = response.FechaIngreso.ToDateTime(),
                Estado = response.Estado
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al actualizar conductor: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _client.EliminarConductorAsync(new ConductorIdRequest { ConductorId = id });
            return true;
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al eliminar conductor: {ex.Message}", ex);
        }
    }
}
