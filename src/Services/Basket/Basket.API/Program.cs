using Basket.API.Endpoints;
using Basket.API.Extentions;
using Basket.API.gRPCServices;
using Basket.API.Repo;
using Basket.API.Services;
using Common.Logging;
using Discount.gRPC.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});


builder.Services.RegisterMapsterConfiguration();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddSingleton<BasketService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["gRPCSettings:DiscountUrl"]);
});
builder.Services.AddScoped<DiscountgRPCService>();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

//builder.Services.AddMassTransitHostedService();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Unhealthy);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddBasketEndpointsAPIs();

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();