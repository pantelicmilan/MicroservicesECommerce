using MediatR;

namespace CatalogService.UseCases.ProductUseCases.Commands.DeleteProductById;

public class DeleteProductByIdCommand : IRequest
{
    public int Id { get; set; }
}
