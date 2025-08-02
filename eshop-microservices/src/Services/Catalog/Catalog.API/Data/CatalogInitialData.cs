using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync(cancellation))
            {
                return; // Data already exists, no need to populate
            }

            session.Store<Product>(GetProconfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetProconfiguredProducts() => new List<Product>
        {
            new Product
            {
                Id = new Guid("c4d573a8-c3da-4507-8e78-d82b6fa059b2"),
                Name = "IPhone 11",
                Category = new List<string> { "Pro Series", "Mobile Devices" },
                Description = "Description for IPhone 11",
                ImageFile = "IPhone11.jpg",
                Price = 69.99m
            },
            new Product
            {
                Id = new Guid("c4d07895-b795-461b-9487-b6652d2104fa"),
                Name = "IPhone 12",
                Category = new List<string> { "Pro Series", "Mobile Devices" },
                Description = "Description for IPhone 12",
                ImageFile = "IPhone12.jpg",
                Price = 99.99m
            },
            new Product
            {
                Id = new Guid("af8f1b79-7f60-466a-bdb8-63d27d3edc3a"),
                Name = "IPhone 13",
                Category = new List<string> { "Mobile Devices" },
                Description = "Description for IPhone 13",
                ImageFile = "IPhone13.jpg",
                Price = 129.99m
            },
            new Product
            {
                Id = new Guid("d3816a01-d894-49e7-abc2-0e9d632a7867"),
                Name = "IPhone 14",
                Category = new List<string> { "Pro Series", "Mobile Devices" },
                Description = "Description for IPhone 14",
                ImageFile = "IPhone14.jpg",
                Price = 179.99m
            },
            new Product
            {
                Id = new Guid("672bf7b5-13e5-45d2-a21f-adc7146b9d30"),
                Name = "Jacket",
                Category = new List<string> { "Mens Clothing", "Upper Wear" },
                Description = "Description for Jacket",
                ImageFile = "Jacket.jpg",
                Price = 89.99m
            },
            new Product
            {
                Id = new Guid("8152b44a-3c1e-4a20-9b89-5ad8cbe412d4"),
                Name = "Muffler",
                Category = new List<string> { "Mens Clothing", "Upper Wear"  },
                Description = "Description for Muffler",
                ImageFile = "Muffler.jpg",
                Price = 9.99m
            },
            new Product
            {
                Id = new Guid("d72d286d-f775-4d2c-b2b1-19b2fff0982e"),
                Name = "Blue Jeans",
                Category = new List<string> { "Mens Clothing", "Bottoms"  },
                Description = "Description for Blue Jeans",
                ImageFile = "Jeans.jpg",
                Price = 12.99m
            }
        };
    }
}
