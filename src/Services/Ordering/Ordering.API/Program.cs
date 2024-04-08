using Common.Logging;
using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ordering.API.Controllers;
using Ordering.API.Endpoints;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSingleton<OrderService>();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config => {
    config.AddConsumer<BasketCheckoutConsumer>();
   
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

//builder.Services.AddMassTransitHostedService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<OrderContext>(failureStatus: HealthStatus.Unhealthy)
    //.AddSqlServer(builder.Configuration["ConnectionStrings:OrderingDb"], name: "SqlServer Health", failureStatus: HealthStatus.Unhealthy)
    .AddElasticsearch(builder.Configuration["ElasticConfiguration:Uri"]!, name: "ElasticSearch Health", failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

app.AddOrderEndpointsAPIs();
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();