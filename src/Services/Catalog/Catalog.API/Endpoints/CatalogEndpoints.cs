using Catalog.API.Entities;
using Catalog.API.Repo;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Endpoints;

public static class CatalogEndpoints
{
    internal static void AddCatalogEndpointsAPIs(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<CatalogService>();

        app.MapGet("/catalog", handler.GetProducts)
            .WithName("GetProducts")
            .WithOpenApi();

        app.MapGet("/catalog/{id:length(24)}", handler.GetProductById)
            .WithName("GetProductById")
            .WithOpenApi();

        app.MapGet("/catalog/GetProductByCategory/{category}", handler.GetProductByCategory)
            .WithName("GetProductByCategory")
            .WithOpenApi();

        app.MapGet("/catalog/GetProductByName/{name}", handler.GetProductByName)
            .WithName("GetProductByName")
            .WithOpenApi();

        app.MapPost("/catalog", handler.CreateProduct)
            .WithName("CreateProduct")
            .WithOpenApi();

        app.MapPut("/catalog", handler.UpdateProduct)
            .WithName("UpdateProduct")
            .WithOpenApi();

        app.MapDelete("/catalog/{id:length(24)}", handler.DeleteProductById)
            .WithName("DeleteProductById")
            .WithOpenApi();
    }
}
