using MediatR;

namespace OrderService.UseCases.CartUseCases.Commands.RemoveProductFromCart;

public class RemoveProductFromCartCommand : IRequest
{
    public int OriginalProductId { get; set; }
}
