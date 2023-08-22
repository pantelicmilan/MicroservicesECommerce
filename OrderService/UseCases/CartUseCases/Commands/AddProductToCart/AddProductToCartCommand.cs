using MediatR;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderService.UseCases.CartUseCases.Commands.AddProductToCart;

public class AddProductToCartCommand : IRequest
{
    public int OriginalProductId { get; set; }
    public int Quantity { get; set; }
}
