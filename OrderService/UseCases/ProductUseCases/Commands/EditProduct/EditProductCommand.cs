using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OrderService.UseCases.ProductUseCases.Commands.EditProduct;

public class EditProductCommand : IRequest
{
    //preko original product id ja
    public int OriginalProductId { get; set; }
    public string ProductName { get; set; }
    public int Qtty { get; set; }
    public decimal Price { get; set; }
}
