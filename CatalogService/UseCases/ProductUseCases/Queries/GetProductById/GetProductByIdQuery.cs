using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Product>
{
    public int Id { get; set; }
}
