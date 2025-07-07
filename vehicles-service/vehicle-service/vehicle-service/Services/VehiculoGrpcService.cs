using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using VehicleService;
using VehicleService.Domain.Entities;
using VehicleService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VehicleService.Services
{
    public class VehiculoGrpcService : VehiculoService.VehiculoServiceBase
    {
        private readonly VehicleDbContext _context;

        public VehiculoGrpcService(VehicleDbContext context)
        {
            _context = context;
        }

        public override async Task<VehiculoDto> CrearVehiculo(VehiculoRequest request, ServerCallContext context)
        {
            try
            {
                // Validar si ya existe la placa
                if (await _context.Vehiculos.AnyAsync(v => v.Placa == request.Placa))
                    throw new RpcException(new Status(StatusCode.AlreadyExists, "Ya existe un vehículo con esa placa."));

                var vehiculo = new Vehiculo
                {
                    Placa = request.Placa,
                    Marca = request.Marca,
                    Modelo = request.Modelo,
                    Anio = request.Anio,
                    TipoMaquinariaId = request.TipoMaquinariaId,
                    EstadoOperativo = true,
                    CapacidadTanqueGalones = (decimal)request.CapacidadTanqueGalones,
                    CombustibleActualGalones = (decimal)request.CombustibleActualGalones
                };

                _context.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();

                return new VehiculoDto
                {
                    Id = vehiculo.Id,
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Anio = vehiculo.Anio,
                    TipoMaquinariaId = vehiculo.TipoMaquinariaId,
                    EstadoOperativo = vehiculo.EstadoOperativo,
                    CapacidadTanqueGalones = Convert.ToDouble(vehiculo.CapacidadTanqueGalones),
                    CombustibleActualGalones = Convert.ToDouble(vehiculo.CombustibleActualGalones)

                };
            }
            catch (RpcException) { throw; } // Si ya es una excepción controlada, la relanza tal cual
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Error al guardar vehículo: {ex.Message}"));
            }
        }

        public override async Task<ListaVehiculos> ListarVehiculos(Empty request, ServerCallContext context)
        {
            try
            {
                var vehiculos = await _context.Vehiculos.ToListAsync();

                var respuesta = new ListaVehiculos();
                respuesta.Vehiculos.AddRange(vehiculos.Select(v => new VehiculoDto
                {
                    Id = v.Id,
                    Placa = v.Placa,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Anio = v.Anio,
                    TipoMaquinariaId = v.TipoMaquinariaId,
                    EstadoOperativo = v.EstadoOperativo,
                    CapacidadTanqueGalones = Convert.ToDouble(v.CapacidadTanqueGalones),
                    CombustibleActualGalones = Convert.ToDouble(v.CombustibleActualGalones)

                }));

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Error al listar vehículos: {ex.Message}"));
            }
        }

        public override async Task<VehiculoDto> ObtenerVehiculoPorId(VehiculoIdRequest request, ServerCallContext context)
        {
            var id = request.Id;

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehículo no encontrado"));

            return new VehiculoDto
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                TipoMaquinariaId = vehiculo.TipoMaquinariaId,
                EstadoOperativo = vehiculo.EstadoOperativo,
                CapacidadTanqueGalones = Convert.ToDouble(vehiculo.CapacidadTanqueGalones),
                CombustibleActualGalones = Convert.ToDouble(vehiculo.CombustibleActualGalones)

            };
        }


        public override async Task<VehiculoDto> EditarVehiculo(EditarVehiculoRequest request, ServerCallContext context)
        {
            try
            {
                var vehiculo = await _context.Vehiculos.FindAsync(request.Id);
                if (vehiculo == null)
                    throw new RpcException(new Status(StatusCode.NotFound, "Vehículo no encontrado"));

                if (request.HasPlaca)
                {
                    bool existeOtra = await _context.Vehiculos
                        .AnyAsync(v => v.Placa == request.Placa && v.Id != request.Id);
                    if (existeOtra)
                        throw new RpcException(new Status(StatusCode.AlreadyExists, "Otra entidad ya usa esa placa."));
                    vehiculo.Placa = request.Placa;
                }

                if (request.HasMarca) vehiculo.Marca = request.Marca;
                if (request.HasModelo) vehiculo.Modelo = request.Modelo;
                if (request.HasAnio) vehiculo.Anio = request.Anio;
                if (request.HasTipoMaquinariaId) vehiculo.TipoMaquinariaId = request.TipoMaquinariaId;
                if (request.HasEstadoOperativo) vehiculo.EstadoOperativo = request.EstadoOperativo;
                if (request.HasCapacidadTanqueGalones) vehiculo.CapacidadTanqueGalones = (decimal)request.CapacidadTanqueGalones;
                if (request.HasCombustibleActualGalones) vehiculo.CombustibleActualGalones = (decimal)request.CombustibleActualGalones;

                await _context.SaveChangesAsync();

                return new VehiculoDto
                {
                    Id = vehiculo.Id,
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Anio = vehiculo.Anio,
                    TipoMaquinariaId = vehiculo.TipoMaquinariaId,
                    EstadoOperativo = vehiculo.EstadoOperativo,
                    CapacidadTanqueGalones = (double)vehiculo.CapacidadTanqueGalones,
                    CombustibleActualGalones = (double)vehiculo.CombustibleActualGalones
                };
            }
            catch (RpcException) { throw; }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Error al editar vehículo: {ex.Message}"));
            }
        }



        public override async Task<Empty> EliminarVehiculo(VehiculoIdRequest request, ServerCallContext context)
        {
            var id = request.Id; 

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Vehículo no encontrado"));

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();

            return new Empty();
        }



        public override async Task<ListaVehiculos> FiltrarVehiculos(FiltroVehiculoRequest request, ServerCallContext context)
        {
            try
            {
                var query = _context.Vehiculos.AsQueryable();

                if (!string.IsNullOrEmpty(request.Placa))
                    query = query.Where(v => v.Placa.Contains(request.Placa));

                if (!string.IsNullOrEmpty(request.Marca))
                    query = query.Where(v => v.Marca.Contains(request.Marca));

                if (!string.IsNullOrEmpty(request.Modelo))
                    query = query.Where(v => v.Modelo.Contains(request.Modelo));

                if (request.Anio != 0)
                    query = query.Where(v => v.Anio == request.Anio);

                if (request.TipoMaquinariaId != 0)
                    query = query.Where(v => v.TipoMaquinariaId == request.TipoMaquinariaId);

                if (request.EstadoOperativo)
                    query = query.Where(v => v.EstadoOperativo == request.EstadoOperativo);

                if (request.CapacidadMinima > 0)
                    query = query.Where(v => v.CapacidadTanqueGalones >= (decimal)request.CapacidadMinima);

                if (request.CombustibleMinimo > 0)
                    query = query.Where(v => v.CombustibleActualGalones >= (decimal)request.CombustibleMinimo);

                var resultados = await query.ToListAsync();

                var respuesta = new ListaVehiculos();
                respuesta.Vehiculos.AddRange(resultados.Select(v => new VehiculoDto
                {
                    Id = v.Id,
                    Placa = v.Placa,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Anio = v.Anio,
                    TipoMaquinariaId = v.TipoMaquinariaId,
                    EstadoOperativo = v.EstadoOperativo,
                    CapacidadTanqueGalones = Convert.ToDouble(v.CapacidadTanqueGalones),
                    CombustibleActualGalones = Convert.ToDouble(v.CombustibleActualGalones)
                }));

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Error al filtrar vehículos: {ex.Message}"));
            }
        }

    }
}
