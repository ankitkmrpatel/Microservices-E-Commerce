using Shopping.API.Aggregator.Services;

namespace Shopping.API.Aggregator.Endpoints;

public static class ShoppingEndpoints
{
    internal static void AddShoppingEndpointsAPIs(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<ShoppingService>();

        app.MapGet("/shopping/{userName}", handler.GetShopping)
            .WithName("GetDiscount")
            .WithOpenApi();
    }
}
