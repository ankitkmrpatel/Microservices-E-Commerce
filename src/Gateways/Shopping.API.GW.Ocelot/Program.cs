using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Host.UseSerilog(SeriLogger.Configure);

builder.Configuration.AddJsonFile("Ocelot.json", true, true);
builder.Configuration
    .AddJsonFile($"Ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.UseOcelot();

app.MapGet("/", () => "Hello There!");

app.Run();