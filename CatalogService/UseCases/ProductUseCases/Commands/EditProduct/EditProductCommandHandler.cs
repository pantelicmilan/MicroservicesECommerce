using CatalogService.EventBus;
using CatalogService.Repositories.Abstractions;
using MediatR;
using MessagingHelper.Events;

namespace CatalogService.UseCases.ProductUseCases.Commands.EditProduct;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public EditProductCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork, 
        IEventBus eventBus)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var currentProduct = await _productRepository.GetProductById(request.Id);
        if (currentProduct == null) throw new Exception("Product not found");
        currentProduct.Name = request.Name;
        currentProduct.Description = request.Description;
        currentProduct.Price = request.Price;
        currentProduct.Quantity = request.Quantity;
        currentProduct.SubcategoryId = request.SubcategoryId;
        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync(new ProductEditedEvent{ 
            Description = currentProduct.Description, 
            Name = currentProduct.Name, 
            Price = currentProduct.Price, 
            ProductId = currentProduct.Id, 
            Quantity = currentProduct.Quantity, 
            SubcategoryId = currentProduct.SubcategoryId
        }
        );
    }
}
