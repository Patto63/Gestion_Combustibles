using DriversService.Domain.Entities;
using DriversService.Persistence;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DriversService.Services;

public class DriverGrpcService : DriverService.DriverServiceBase
{
    private readonly DriversDbContext _context;

    public DriverGrpcService(DriversDbContext context)
    {
        _context = context;
    }

    // Helpers
    private static ConductorDto ToConductorDto(Conductor c) => new()
    {
        ConductorId = c.ConductorId,
        Codigo = c.Codigo,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        NumeroDocumento = c.NumeroDocumento,
        NumeroLicencia = c.NumeroLicencia,
        TipoLicencia = c.TipoLicencia,
        FechaExpiracionLicencia = Timestamp.FromDateTime(c.FechaExpiracionLicencia.ToUniversalTime()),
        FechaNacimiento = Timestamp.FromDateTime(c.FechaNacimiento.ToUniversalTime()),
        NumeroTelefono = c.NumeroTelefono ?? string.Empty,
        CorreoElectronico = c.CorreoElectronico ?? string.Empty,
        Direccion = c.Direccion ?? string.Empty,
        FechaIngreso = Timestamp.FromDateTime(c.FechaIngreso.ToUniversalTime()),
        Estado = c.Estado
    };

    private static EspecialidadDto ToEspecialidadDto(EspecialidadConductor e) => new()
    {
        EspecialidadId = e.EspecialidadId,
        ConductorId = e.ConductorId,
        TipoMaquinaria = e.TipoMaquinaria,
        Descripcion = e.Descripcion ?? string.Empty,
        NumeroCertificacion = e.NumeroCertificacion ?? string.Empty,
        FechaCertificacion = Timestamp.FromDateTime(e.FechaCertificacion.ToUniversalTime()),
        ExpiracionCertificacion = Timestamp.FromDateTime(e.ExpiracionCertificacion.ToUniversalTime())
    };

    private static AsignacionDto ToAsignacionDto(HistorialAsignacionConductor a) => new()
    {
        AsignacionId = a.AsignacionId,
        ConductorId = a.ConductorId,
        CodigoVehiculo = a.CodigoVehiculo,
        TipoMaquinaria = a.TipoMaquinaria,
        FechaInicioAsignacion = Timestamp.FromDateTime(a.FechaInicioAsignacion.ToUniversalTime()),
        FechaFinAsignacion = a.FechaFinAsignacion != null ? Timestamp.FromDateTime(a.FechaFinAsignacion.Value.ToUniversalTime()) : null,
        Estado = a.Estado ?? string.Empty,
        CreadoPor = a.CreadoPor ?? string.Empty
    };

    // Conductores CRUD
    public override async Task<ConductorDto> CrearConductor(ConductorCreateRequest request, ServerCallContext context)
    {
        var entity = new Conductor
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            NumeroDocumento = request.NumeroDocumento,
            NumeroLicencia = request.NumeroLicencia,
            TipoLicencia = request.TipoLicencia,
            FechaExpiracionLicencia = request.FechaExpiracionLicencia.ToDateTime(),
            FechaNacimiento = request.FechaNacimiento.ToDateTime(),
            NumeroTelefono = request.NumeroTelefono,
            CorreoElectronico = request.CorreoElectronico,
            Direccion = request.Direccion,
            FechaIngreso = request.FechaIngreso.ToDateTime(),
            Estado = true,
            CreadoEn = DateTime.UtcNow
        };
        _context.Conductores.Add(entity);
        await _context.SaveChangesAsync();
        return ToConductorDto(entity);
    }

    public override async Task<ConductorDto> ObtenerConductorPorId(ConductorIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Conductores.FindAsync(request.ConductorId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Conductor no encontrado"));
        return ToConductorDto(entity);
    }

    public override async Task<ListaConductores> ListarConductores(Empty request, ServerCallContext context)
    {
        var list = await _context.Conductores.ToListAsync();
        var res = new ListaConductores();
        res.Conductores.AddRange(list.Select(ToConductorDto));
        return res;
    }

    public override async Task<ConductorDto> EditarConductor(ConductorUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.Conductores.FindAsync(request.ConductorId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Conductor no encontrado"));

        if (request.HasCodigo) entity.Codigo = request.Codigo;
        if (request.HasNombre) entity.Nombre = request.Nombre;
        if (request.HasApellido) entity.Apellido = request.Apellido;
        if (request.HasNumeroDocumento) entity.NumeroDocumento = request.NumeroDocumento;
        if (request.HasNumeroLicencia) entity.NumeroLicencia = request.NumeroLicencia;
        if (request.HasTipoLicencia) entity.TipoLicencia = request.TipoLicencia;
        if (request.HasFechaExpiracionLicencia) entity.FechaExpiracionLicencia = request.FechaExpiracionLicencia.ToDateTime();
        if (request.HasFechaNacimiento) entity.FechaNacimiento = request.FechaNacimiento.ToDateTime();
        if (request.HasNumeroTelefono) entity.NumeroTelefono = request.NumeroTelefono;
        if (request.HasCorreoElectronico) entity.CorreoElectronico = request.CorreoElectronico;
        if (request.HasDireccion) entity.Direccion = request.Direccion;
        if (request.HasEstado) entity.Estado = request.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToConductorDto(entity);
    }

    public override async Task<Empty> EliminarConductor(ConductorIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Conductores.FindAsync(request.ConductorId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Conductor no encontrado"));
        _context.Conductores.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    // Especialidades CRUD
    public override async Task<EspecialidadDto> CrearEspecialidad(EspecialidadCreateRequest request, ServerCallContext context)
    {
        var entity = new EspecialidadConductor
        {
            ConductorId = request.ConductorId,
            TipoMaquinaria = request.TipoMaquinaria,
            Descripcion = request.Descripcion,
            NumeroCertificacion = request.NumeroCertificacion,
            FechaCertificacion = request.FechaCertificacion.ToDateTime(),
            ExpiracionCertificacion = request.ExpiracionCertificacion.ToDateTime(),
            CreadoEn = DateTime.UtcNow
        };
        _context.Especialidades.Add(entity);
        await _context.SaveChangesAsync();
        return ToEspecialidadDto(entity);
    }

    public override async Task<EspecialidadDto> ObtenerEspecialidadPorId(EspecialidadIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Especialidades.FindAsync(request.EspecialidadId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Especialidad no encontrada"));
        return ToEspecialidadDto(entity);
    }

    public override async Task<ListaEspecialidades> ListarEspecialidades(Empty request, ServerCallContext context)
    {
        var list = await _context.Especialidades.ToListAsync();
        var res = new ListaEspecialidades();
        res.Especialidades.AddRange(list.Select(ToEspecialidadDto));
        return res;
    }

    public override async Task<EspecialidadDto> EditarEspecialidad(EspecialidadUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.Especialidades.FindAsync(request.EspecialidadId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Especialidad no encontrada"));

        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasDescripcion) entity.Descripcion = request.Descripcion;
        if (request.HasNumeroCertificacion) entity.NumeroCertificacion = request.NumeroCertificacion;
        if (request.HasFechaCertificacion) entity.FechaCertificacion = request.FechaCertificacion.ToDateTime();
        if (request.HasExpiracionCertificacion) entity.ExpiracionCertificacion = request.ExpiracionCertificacion.ToDateTime();
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToEspecialidadDto(entity);
    }

    public override async Task<Empty> EliminarEspecialidad(EspecialidadIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Especialidades.FindAsync(request.EspecialidadId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Especialidad no encontrada"));
        _context.Especialidades.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }

    // Asignaciones CRUD
    public override async Task<AsignacionDto> CrearAsignacion(AsignacionCreateRequest request, ServerCallContext context)
    {
        var entity = new HistorialAsignacionConductor
        {
            ConductorId = request.ConductorId,
            CodigoVehiculo = request.CodigoVehiculo,
            TipoMaquinaria = request.TipoMaquinaria,
            FechaInicioAsignacion = request.FechaInicioAsignacion.ToDateTime(),
            FechaFinAsignacion = request.FechaFinAsignacion.ToDateTime(),
            Estado = request.Estado,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = request.CreadoPor
        };
        _context.Asignaciones.Add(entity);
        await _context.SaveChangesAsync();
        return ToAsignacionDto(entity);
    }

    public override async Task<AsignacionDto> ObtenerAsignacionPorId(AsignacionIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Asignaciones.FindAsync(request.AsignacionId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Asignacion no encontrada"));
        return ToAsignacionDto(entity);
    }

    public override async Task<ListaAsignaciones> ListarAsignaciones(Empty request, ServerCallContext context)
    {
        var list = await _context.Asignaciones.ToListAsync();
        var res = new ListaAsignaciones();
        res.Asignaciones.AddRange(list.Select(ToAsignacionDto));
        return res;
    }

    public override async Task<AsignacionDto> EditarAsignacion(AsignacionUpdateRequest request, ServerCallContext context)
    {
        var entity = await _context.Asignaciones.FindAsync(request.AsignacionId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Asignacion no encontrada"));

        if (request.HasCodigoVehiculo) entity.CodigoVehiculo = request.CodigoVehiculo;
        if (request.HasTipoMaquinaria) entity.TipoMaquinaria = request.TipoMaquinaria;
        if (request.HasFechaInicioAsignacion) entity.FechaInicioAsignacion = request.FechaInicioAsignacion.ToDateTime();
        if (request.HasFechaFinAsignacion) entity.FechaFinAsignacion = request.FechaFinAsignacion.ToDateTime();
        if (request.HasEstado) entity.Estado = request.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToAsignacionDto(entity);
    }

    public override async Task<Empty> EliminarAsignacion(AsignacionIdRequest request, ServerCallContext context)
    {
        var entity = await _context.Asignaciones.FindAsync(request.AsignacionId);
        if (entity == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Asignacion no encontrada"));
        _context.Asignaciones.Remove(entity);
        await _context.SaveChangesAsync();
        return new Empty();
    }
}
