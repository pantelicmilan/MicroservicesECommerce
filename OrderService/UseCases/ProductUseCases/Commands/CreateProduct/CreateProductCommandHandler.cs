using MediatR;
using OrderService.Repositories.Abstractions;
using OrderService.Entities;

namespace OrderService.UseCases.ProductUseCases.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _productRepository.CreateProduct(new Product { 
            Price = request.Price, 
            ProductName = request.ProductName, 
            Qtty = request.Qtty,
            OriginalProductId = request.OriginalProductId
        });
        await _unitOfWork.SaveChangesAsync();
    }
}
