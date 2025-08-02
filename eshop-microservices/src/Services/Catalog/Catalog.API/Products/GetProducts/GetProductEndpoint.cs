using System.Collections;

namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
            async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();

                var result = await sender.Send(query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithSummary("Get all products")
            .WithDescription("Retrieve a list of all products in the catalog")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
    }
}