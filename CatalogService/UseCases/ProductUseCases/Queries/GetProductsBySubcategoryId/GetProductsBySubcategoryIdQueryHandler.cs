using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductsBySubcategoryId;

public class GetProductsBySubcategoryIdQueryHandler : IRequestHandler<GetProductsBySubcategoryIdQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsBySubcategoryIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Product>> Handle(GetProductsBySubcategoryIdQuery request, CancellationToken cancellationToken)
    {
        var products = _productRepository.GetProductsBySubcategoryId(request.SubcategoryId);
        if (products == null) throw new Exception("Subcategory Not found");
        return Task.FromResult(products);
    }

}
