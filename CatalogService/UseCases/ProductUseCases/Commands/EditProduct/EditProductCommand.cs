using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Commands.EditProduct;

public class EditProductCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int SubcategoryId { get; set; }
    public int Quantity { get; set; }
}
