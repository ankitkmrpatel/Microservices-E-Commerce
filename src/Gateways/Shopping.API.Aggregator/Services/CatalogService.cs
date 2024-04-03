using Shopping.API.Aggregator.Extention;
using Shopping.API.Aggregator.Models;

namespace Shopping.API.Aggregator.Services;

public class CatalogService(HttpClient client) : ICatalogService
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        var response = await _client.GetAsync("/products");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        var response = await _client.GetAsync($"/products/{id}");
        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        var response = await _client.GetAsync($"/products/category/{category}");
        return await response.ReadContentAs<List<CatalogModel>>();
    }
}

public interface ICatalogService
{
    Task<IEnumerable<CatalogModel>> GetCatalog();
    Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
    Task<CatalogModel> GetCatalog(string id);
}
