namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule 
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteProductResponse>();
                return response.IsSuccess
                    ? Results.Ok(response)
                    : Results.NotFound(new { Message = "Product not found" });
            })
                .WithName("DeleteProduct")
                .WithSummary("Delete a product")
                .WithDescription("Remove a product from the catalog by its unique identifier")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
                .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
