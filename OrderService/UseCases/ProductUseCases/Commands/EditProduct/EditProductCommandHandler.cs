using MediatR;
using OrderService.Repositories.Abstractions;
using System.CodeDom;

namespace OrderService.UseCases.ProductUseCases.Commands.EditProduct;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetProductByOriginalProductId(request.OriginalProductId);
        if (product == null) throw new Exception("Product not found");
        product.Price = request.Price;
        product.ProductName = request.ProductName;
        product.Qtty = request.Qtty;
        await _unitOfWork.SaveChangesAsync();
    }
}
