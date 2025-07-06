using RoutesService.Persistence;
using RoutesService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<RoutesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RoutesDbContext>();
    db.Database.EnsureCreated();
}

app.MapGrpcService<RouteGrpcService>();
app.MapGet("/", () => "gRPC service for routes");

app.Run();
