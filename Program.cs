using RoomHub.Api.Features;
using RoomHub.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RoomHub") ?? "Data Source=roomhub.db";

builder.Services.AddProblemDetails();
builder.Services.AddSingleton(TimeProvider.System);   // the only source of "now" (R5)
builder.Services.AddPersistence(connectionString);
builder.Services.AddFeatures();

var app = builder.Build();

app.UseExceptionHandler();   // anything unexpected -> 500 ProblemDetails (business errors never get here)

// Create the SQLite file and tables on first start. Data stays in the file between restarts.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<RoomHubDbContext>().Database.EnsureCreated();
}

app.MapGet("/", () => "RoomHub API is running.");
app.MapFeatures();

app.Run();
