using CatalogService.EventBus;
using CatalogService.Repositories.Abstractions;
using MediatR;
using MessagingHelper.Events;

namespace CatalogService.UseCases.ProductUseCases.Commands.DeleteProductById;

public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public DeleteProductByIdCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(request.Id);
        if (product == null) throw new Exception("Product not found!");
        _productRepository.DeleteProduct(product);
        await _unitOfWork.SaveChangesAsync();
        await _eventBus.PublishAsync<ProductDeletedEvent>(
            new ProductDeletedEvent { ProductId = product.Id }
            );
    }
}
