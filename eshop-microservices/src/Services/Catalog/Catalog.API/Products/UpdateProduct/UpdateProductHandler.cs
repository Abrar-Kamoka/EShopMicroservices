using FluentValidation;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile)
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product Name is required.")
                .Length(3, 150).WithMessage("Name must be between 3 and 150 characters.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product Price must be greater than zero.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Product Category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product ImageFile is required.");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null)
            {
                logger.LogWarning("Product with Id {Id} not found", command.Id);
                //return new UpdateProductResult(false);
                //throw new KeyNotFoundException($"Product with Id {command.Id} not found");
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Category = command.Category;
            product.ImageFile = command.ImageFile;
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
