using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repo;
using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Services;

public class BasketService
{
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetBasket(IBasketRepository _repo, string userName)
    {
        var basket = await _repo.GetBasket(userName);
        return Results.Ok(basket ?? new ShoppingCart(userName));
    }

    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IResult> UpdateBasket(IBasketRepository _repo, IServiceProvider provider, [FromBody] ShoppingCart basket)
    {
        using (var scope = provider.CreateScope())
        {
            DiscountgRPCService _discountgRPCService = scope.ServiceProvider.GetRequiredService<DiscountgRPCService>();
            // Communicate with Discount.Grpc and calculate lastest prices of products into sc
            foreach (var item in basket.Items)
            {
                var coupon = await _discountgRPCService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
        }

        return Results.Ok(await _repo.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IResult> DeleteBasket(IBasketRepository _repo, string userName)
    {
        await _repo.DeleteBasket(userName);
        return Results.Ok();
    }

    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IResult> Checkout(IBasketRepository _repo, IPublishEndpoint _publishEndpoint, [FromBody] BasketCheckout basketCheckout)
    {
        // get existing basket with total price            
        // Set TotalPrice on basketCheckout eventMessage
        // send checkout event to rabbitmq
        // remove the basket

        //get existing basket with total price
        var basket = await _repo.GetBasket(basketCheckout.UserName);
        if (basket == null)
        {
            return Results.BadRequest();
        }

        // send checkout event to rabbitmq
        var eventMessage = basketCheckout.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);

        //// remove the basket
        await _repo.DeleteBasket(basket.UserName);

        return Results.Accepted();
    }
}
