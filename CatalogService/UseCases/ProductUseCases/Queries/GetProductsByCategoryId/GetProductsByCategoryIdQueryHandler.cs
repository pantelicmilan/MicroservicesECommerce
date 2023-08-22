using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductsByCategoryId;

public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;
    public GetProductsByCategoryIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsByCategoryId(request.CategoryId);
        if (products == null) throw new Exception("Category not found!");
        return products;
    }
}
