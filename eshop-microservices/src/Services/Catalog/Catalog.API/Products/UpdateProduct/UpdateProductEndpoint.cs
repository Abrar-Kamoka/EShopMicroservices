
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(string Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile);
    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return response.IsSuccess
                    ? Results.Ok(response)
                    : Results.NotFound(new { Message = "Product not found" });
            })
                .WithName("UpdateProduct")
                .WithSummary("Update an existing product")
                .WithDescription("Update Product Description")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
                .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
