
namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest();
     public record GetProductByCateogryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",
                async (string category, ISender sender) =>
                {
                    var result = await sender.Send(new GetProductByCategoryQuery(category));
                    var response = result.Adapt<GetProductByCateogryResponse>();
                    return Results.Ok(response);
                })
                .WithName("GetProductByCategory")
                .WithSummary("Get products by category")
                .WithDescription("Retrieve a list of products from the catalog by category")
                .Produces<GetProductByCateogryResponse>(StatusCodes.Status200OK)
                .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
