using MediatR;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.ProductUseCases.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    async Task<Product> IRequestHandler<GetProductByIdQuery, Product>.Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByOriginalProductId(request.ProductId);
        if (product == null) throw new Exception("Product Not Found!");
        return product;
    }
}
