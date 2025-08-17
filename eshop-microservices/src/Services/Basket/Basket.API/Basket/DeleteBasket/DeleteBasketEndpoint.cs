using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.DeleteBasket
{

    //public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}",
                async (string userName, ISender sender) =>
                {
                    var command = new DeleteBasketCommand(userName);
                    var result = await sender.Send(command);
                    var response = result.Adapt<DeleteBasketResponse>();
                    return result.IsSuccess ? Results.Ok(response) : Results.NotFound(new { Message = "User not found" });
                })
                .WithName("DeleteBasket")
                .WithSummary("Delete a user's shopping cart")
                .WithDescription("Deletes the shopping cart for a specified user by their username.")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
