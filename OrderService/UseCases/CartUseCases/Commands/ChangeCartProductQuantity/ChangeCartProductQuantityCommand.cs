using MassTransit;
using MediatR;

namespace OrderService.UseCases.CartUseCases.Commands.ChangeCartProductQuantity;

public class ChangeCartProductQuantityCommand : IRequest
{
    public int NewQuantity { get; set; }
    public int OriginalProductId { get; set; }
}
