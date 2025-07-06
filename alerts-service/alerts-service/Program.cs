using AlertsService.Persistence;
using AlertsService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<AlertsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AlertsDbContext>();
    db.Database.EnsureCreated();
}

app.MapGrpcService<AlertsGrpcService>();
app.MapGet("/", () => "gRPC service for alerts");

app.Run();
