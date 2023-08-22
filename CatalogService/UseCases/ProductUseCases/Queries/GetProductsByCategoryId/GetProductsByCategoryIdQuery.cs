using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductsByCategoryId;

public class GetProductsByCategoryIdQuery : IRequest<List<Product>>
{
    public int CategoryId { get; set; }
}
