using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.gRPC.Protos;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Shopping cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required.");
            RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Shopping cart must contain at least one item.");
        }
    }
    // 12-7
    public class StoreBasketCommandHandler (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // TODO: Communicate with Discount.gRPC and calculate latest prices of products into the shopping cart
            await DeductDiscount(command.Cart,  cancellationToken);

            // TODO: store basket in database (use Marten upsert - if exists = update, if not exists = insert ) and update cache if any
            await repository.StoreBasketAsync(command.Cart, cancellationToken);
            //return Task.FromResult(new StoreBasketResult(cart.UserName));
            return new StoreBasketResult(command.Cart.UserName);
        }

        public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
                var coupon = await discountProto.GetDiscountAsync(discountRequest);
                item.Price -= coupon.Amount;
            }
        }
    }
}
