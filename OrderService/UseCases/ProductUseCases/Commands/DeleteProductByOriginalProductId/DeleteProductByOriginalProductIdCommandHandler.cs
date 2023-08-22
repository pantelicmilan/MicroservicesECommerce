using MediatR;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.ProductUseCases.Commands.DeleteProductById;

public class DeleteProductByOriginalProductIdCommandHandler : IRequestHandler<DeleteProductByOriginalProductIdCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductByOriginalProductIdCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProductByOriginalProductIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByOriginalProductId(request.OriginalProductId);
        if (product == null) throw new Exception("Product not found");
        _productRepository.DeleteProduct(product);
        await _unitOfWork.SaveChangesAsync();
    }
}
