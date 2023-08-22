using MediatR;
using OrderService.Entities;

namespace OrderService.UseCases.ProductUseCases.Queries.GetProductByOriginalProductId;

public class GetProductByOriginalProductIdQuery : IRequest<Product>
{
    public int OriginalProductId { get; set; }
}
