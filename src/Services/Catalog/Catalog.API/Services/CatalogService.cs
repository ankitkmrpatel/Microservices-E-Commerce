using Catalog.API.Entities;
using Catalog.API.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Services;

public class CatalogService  //: ICatalogService
{
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetProducts(IProductRepository _repo)
    {
        var products = await _repo.GetProducts();
        return Results.Ok(products);
    }


    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetProductById(IProductRepository _repo, ILogger<CatalogService> _logger, string id)
    {
        var product = await _repo.GetProduct(id);

        if (product == null)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return Results.NotFound();
        }

        return Results.Ok(product);
    }


    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetProductByCategory(IProductRepository _repo, string category)
    {
        var products = await _repo.GetProductByCategory(category);
        return Results.Ok(products);
    }

    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetProductByName(IProductRepository _repo, ILogger<CatalogService> _logger, string name)
    {
        var items = await _repo.GetProductByName(name);
        if (items == null)
        {
            _logger.LogError($"Products with name: {name} not found.");
            return Results.NotFound();
        }
        return Results.Ok(items);
    }

    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IResult> CreateProduct(IProductRepository _repo, [FromBody] Product product)
    {
        await _repo.CreateProduct(product);
        return Results.CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IResult> UpdateProduct(IProductRepository _repo, [FromBody] Product product)
    {
        return Results.Ok(await _repo.UpdateProduct(product));
    }

    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IResult> DeleteProductById(IProductRepository _repo, string id)
    {
        return Results.Ok(await _repo.DeleteProduct(id));
    }
}
