using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductsBySubcategoryId;

public class GetProductsBySubcategoryIdQuery: IRequest<List<Product>>
{
    public int SubcategoryId { get; set; }
}
