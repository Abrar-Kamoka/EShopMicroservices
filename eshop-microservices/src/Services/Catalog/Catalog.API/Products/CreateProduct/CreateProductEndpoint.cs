
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);


    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // 6-1 completted
            app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateProductCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<CreateProductResponse>();

                    return Results.Created($"/products/{response.Id}", response);
                })
                .WithName("CreateProduct")
                .WithSummary("Create a new product")
                .WithDescription("Create Product Description")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
