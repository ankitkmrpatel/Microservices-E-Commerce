using Common.Logging;
using Discount.gRPC.Extentions;
using Discount.gRPC.Repo;
using Discount.gRPC.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.
builder.Services.AddScoped<IDatabaseConfiguration, DatabaseConfiguration>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<DiscountService>();

builder.Services.RegisterMapsterConfiguration();

builder.Services.AddGrpc();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration["DatabaseSettings:ConnectionString"], name: "Postgresql  Health", failureStatus: HealthStatus.Unhealthy);


var app = builder.Build();

app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
