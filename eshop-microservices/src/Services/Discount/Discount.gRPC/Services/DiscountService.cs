using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Discount.gRPC.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService (DiscountContext dbcontext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var coupon = await dbcontext.Coupons
                    .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
                if (coupon == null)
                {
                    coupon = new Coupon { ProductName = "No Discount", Description = "No Discount Desc", Amount = 0 };
                }

                logger.LogInformation("Discount retrieved for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);

                var couponModel = coupon.Adapt<CouponModel>();
                return couponModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting discount");
                throw new RpcException(new Status(StatusCode.Internal, $"Server error: {ex.Message}"));
            }
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            try
            {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));

            dbcontext.Coupons.Add(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation("Discount successfully created for ProductName : {ProductName}", coupon.ProductName);
            
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating discount");
                throw new RpcException(new Status(StatusCode.Internal, $"Server error: {ex.Message}"));
            }
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon data is null"));

            dbcontext.Coupons.Update(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation("Discount successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation("Discount successfully deleted. ProductName : {ProductName}", request.ProductName);
            
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
