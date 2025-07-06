using FuelService.Persistence;
using FuelService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<FuelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FuelDbContext>();
    db.Database.EnsureCreated();
}

app.MapGrpcService<FuelGrpcService>();
app.MapGet("/", () => "gRPC service for fuel management");

app.Run();
