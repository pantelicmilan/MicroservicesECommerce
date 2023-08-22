using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(request.Id);
        if (product == null) throw new Exception("Product not found!");
        return product;
    }
}
