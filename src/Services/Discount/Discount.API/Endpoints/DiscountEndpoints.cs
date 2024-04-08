using Discount.API.Services;

namespace Discount.API.Endpoints;

public static class DiscountEndpoints
{
    internal static void AddDiscountEndpointsAPIs(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<DiscountService>();

        app.MapGet("/discount/{productName}", handler.GetDiscount)
            .WithName("GetDiscount")
            .WithOpenApi();

        app.MapPost("/discount", handler.CreateDiscount)
            .WithName("CreateDiscount")
            .WithOpenApi();

        app.MapPut("/discount", handler.UpdateDiscount)
            .WithName("UpdateDiscount")
            .WithOpenApi();

        app.MapDelete("/discount/{productName}", handler.DeleteDiscount)
            .WithName("DeleteProductById")
            .WithOpenApi();
    }
}
