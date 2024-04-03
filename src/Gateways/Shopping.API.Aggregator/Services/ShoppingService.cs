using Microsoft.AspNetCore.Mvc;
using Shopping.API.Aggregator.Models;
using System.Net;

namespace Shopping.API.Aggregator.Services;

public class ShoppingService  //: IShoppingService
{
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetShopping(IBasketService _basketService, ICatalogService _catalogService, IOrderService _orderService, 
        string userName)
    {
        var orders = await _orderService.GetOrdersByUserName(userName);
        var basket = await _basketService.GetBasket(userName);

        foreach (var item in basket.Items)
        {
            var product = await _catalogService.GetCatalog(item.ProductId);

            // set additional product fields
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.Summary = product.Summary;
            item.Description = product.Description;
            item.ImageFile = product.ImageFile;
        }

        var shoppingModel = new ShoppingModel
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Results.Ok(shoppingModel);
    }

}
