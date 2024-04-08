using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repo;
using Grpc.Core;
using Mapster;

namespace Discount.gRPC.Services;

public class DiscountService(IDiscountRepository repo, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repo = repo;
    private readonly ILogger<DiscountService> _logger = logger;

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repo.GetDiscount(request.ProductName) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        await _repo.CreateDiscount(coupon);
        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        await _repo.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _repo.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };

        return response;
    }
}
