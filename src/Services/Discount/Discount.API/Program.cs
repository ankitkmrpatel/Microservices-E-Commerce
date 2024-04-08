using Discount.API.Services;
using Discount.API.Repo;
using Discount.API.Endpoints;
using Discount.API.Extentions;
using Serilog;
using Common.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddScoped<IDatabaseConfiguration, DatabaseConfiguration>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddSingleton<DiscountService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration["DatabaseSettings:ConnectionString"], name: "Postgresql  Health", failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();

app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddDiscountEndpointsAPIs();
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();