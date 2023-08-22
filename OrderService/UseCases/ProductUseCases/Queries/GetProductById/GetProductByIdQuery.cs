using MediatR;
using OrderService.Entities;

namespace OrderService.UseCases.ProductUseCases.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Product>
{
    public int ProductId { get; set; }
}
