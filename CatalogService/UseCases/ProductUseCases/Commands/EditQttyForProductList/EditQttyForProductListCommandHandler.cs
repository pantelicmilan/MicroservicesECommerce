using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Commands.EditQttyForProductList;

public class EditQttyForProductListCommandHandler : IRequestHandler<EditQttyForProductListCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditQttyForProductListCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditQttyForProductListCommand request, CancellationToken cancellationToken)
    {
        if (request.ProductAfterOrders == null) throw new Exception("Product After Orders empty!");
        await _productRepository
            .EditProductsQuantityWithProductAfterOrderList(request.ProductAfterOrders);
        await _unitOfWork.SaveChangesAsync();
    }
}
