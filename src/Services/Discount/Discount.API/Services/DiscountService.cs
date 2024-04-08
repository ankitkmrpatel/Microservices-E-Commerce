using Discount.API.Entities;
using Discount.API.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Services;

public class DiscountService  //: IDiscountService
{
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<IResult> GetDiscount(IDiscountRepository _repo, string productName)
    {
        var discount = await _repo.GetDiscount(productName);
        return Results.Ok(discount);
    }

    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<IResult> CreateDiscount(IDiscountRepository _repo, [FromBody] Coupon coupon)
    {
        await _repo.CreateDiscount(coupon);
        return Results.CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
    }

    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<IResult> UpdateDiscount(IDiscountRepository _repo, [FromBody] Coupon coupon)
    {
        return Results.Ok(await _repo.UpdateDiscount(coupon));
    }

    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IResult> DeleteDiscount(IDiscountRepository _repo, string productName)
    {
        return Results.Ok(await _repo.DeleteDiscount(productName));
    }
}
