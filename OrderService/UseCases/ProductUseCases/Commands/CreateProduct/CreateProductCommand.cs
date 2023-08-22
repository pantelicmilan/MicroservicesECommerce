using MediatR;

namespace OrderService.UseCases.ProductUseCases.Commands.CreateProduct;

public class CreateProductCommand : IRequest
{
    public string ProductName { get; set; }
    public int Qtty { get; set; }
    public decimal Price { get; set; }
    public int OriginalProductId { get; set; }
}
