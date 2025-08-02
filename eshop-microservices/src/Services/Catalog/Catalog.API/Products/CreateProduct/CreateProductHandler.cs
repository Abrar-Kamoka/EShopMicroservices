
using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(150).WithMessage("Product name must not exceed 100 characters.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Product category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product image file is required.")
                .Must(file => file.EndsWith(".jpg") || file.EndsWith(".png"))
                .WithMessage("Product image file must be a valid image format (jpg or png).");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
        }
    }

    internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create product entity from command object
            // save to database
            // return CreateProductResult result

            //logger.LogInformation("CreateProductCommandHandler is Handling CreateProductCommand called with {@Command}", command);

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }
    }
}
