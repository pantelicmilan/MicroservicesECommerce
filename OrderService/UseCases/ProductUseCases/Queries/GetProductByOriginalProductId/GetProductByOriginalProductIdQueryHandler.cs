using MediatR;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.ProductUseCases.Queries.GetProductByOriginalProductId;

public class GetProductByOriginalProductIdQueryHandler : IRequestHandler<GetProductByOriginalProductIdQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public async Task<Product> Handle(GetProductByOriginalProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetProductByOriginalProductId(request.OriginalProductId);
        if (product == null) throw new Exception("Product not found!");
        return product;
    }
}
