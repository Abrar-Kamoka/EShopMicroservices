
namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIdRequest();
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProductById")
                .WithSummary("Get a product by Id")
                .WithDescription("Retrieve a product from the catalog by its unique identifier")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
