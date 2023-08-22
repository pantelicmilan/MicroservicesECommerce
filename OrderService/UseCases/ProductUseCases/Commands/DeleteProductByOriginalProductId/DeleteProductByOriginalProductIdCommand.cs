using MediatR;

namespace OrderService.UseCases.ProductUseCases.Commands.DeleteProductById;

public class DeleteProductByOriginalProductIdCommand : IRequest
{
    public int OriginalProductId { get; set; }
}
