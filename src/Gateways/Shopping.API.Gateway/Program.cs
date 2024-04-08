using Microsoft.Extensions.Configuration;
using Serilog;
using Common.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Configuration.AddJsonFile("Ocelot.json", true, true);
builder.Configuration
    .AddJsonFile($"Ocelot.{builder.Environment.EnvironmentName}.json", true, true);

//builder.Host.ConfigureLogging((hosintContext, loggingBuilder) =>
//{
//    loggingBuilder.AddConfiguration(hosintContext.Configuration.GetSection("Logging"));
//    loggingBuilder.AddConsole();
//    loggingBuilder.AddDebug();
//});

//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle()); ;

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();
