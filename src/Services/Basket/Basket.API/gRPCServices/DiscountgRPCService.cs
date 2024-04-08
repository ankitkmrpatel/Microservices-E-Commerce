using Discount.gRPC.Protos;
using Discount.gRPC.Protos;

namespace Basket.API.gRPCServices;

public class DiscountgRPCService(DiscountProtoService.DiscountProtoServiceClient client)
{
    private readonly DiscountProtoService.DiscountProtoServiceClient client = client;

    public async Task<CouponModel> GetDiscount(string productName)
    {
        return await client.GetDiscountAsync(new GetDiscountRequest()
        {
            ProductName = productName
        });
    }
}
