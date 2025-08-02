using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    public class GetProductByCategoryHandler(IDocumentSession session, ILogger<GetProductByCategoryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}
