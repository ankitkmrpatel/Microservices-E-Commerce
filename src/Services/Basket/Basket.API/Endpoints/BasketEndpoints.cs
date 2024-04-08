using Basket.API.Services;

namespace Basket.API.Endpoints;

public static class BasketEndpoints
{
    internal static void AddBasketEndpointsAPIs(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<BasketService>();

        app.MapGet("/basket/{userName}", handler.GetBasket)
            .WithName("GetBasket")
            .WithOpenApi();

        app.MapPost("/basket", handler.UpdateBasket)
            .WithName("UpdateBasket")
            .WithOpenApi();

        app.MapDelete("/basket/{userName}", handler.DeleteBasket)
            .WithName("DeleteBasket")
            .WithOpenApi();

        app.MapPost("/basket/Checkout", handler.Checkout)
            .WithName("Checkout")
            .WithOpenApi();
    }
}
