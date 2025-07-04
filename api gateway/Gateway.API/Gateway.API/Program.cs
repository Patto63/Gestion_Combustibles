using Gateway.API.GrpcClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Registro Cliente gRPC
builder.Services.AddSingleton<VehiculoGrpcClient>();
builder.Services.AddSingleton<DriverGrpcClient>();

builder.Services.AddSingleton<FuelGrpcClient>();
builder.Services.AddSingleton<RouteGrpcClient>();
builder.Services.AddSingleton<AlertsGrpcClient>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
