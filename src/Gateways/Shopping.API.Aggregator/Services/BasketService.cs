using Shopping.API.Aggregator.Extention;
using Shopping.API.Aggregator.Models;
using System.Net.Http;

namespace Shopping.API.Aggregator.Services;

public class BasketService(HttpClient client) : IBasketService
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<BasketModel> GetBasket(string userName)
    {
        var response = await _client.GetAsync($"/Basket/{userName}");
        return await response.ReadContentAs<BasketModel>();
    }
}

public interface IBasketService
{
    Task<BasketModel> GetBasket(string userName);
}
