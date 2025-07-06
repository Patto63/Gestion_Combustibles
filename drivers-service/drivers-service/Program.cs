using DriversService.Persistence;
using DriversService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<DriversDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DriversDbContext>();
    db.Database.EnsureCreated();
}

app.MapGrpcService<DriverGrpcService>();
app.MapGet("/", () => "gRPC service for drivers");

app.Run();
