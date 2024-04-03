using Shopping.API.Aggregator.Extention;
using Shopping.API.Aggregator.Models;

namespace Shopping.API.Aggregator.Services;

public class OrderService(HttpClient client) : IOrderService
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
        var response = await _client.GetAsync($"/Order/{userName}");
        return await response.ReadContentAs<List<OrderResponseModel>>();
    }
}

public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
}
