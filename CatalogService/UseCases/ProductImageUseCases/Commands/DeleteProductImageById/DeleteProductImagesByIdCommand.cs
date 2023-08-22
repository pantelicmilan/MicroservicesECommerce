using MediatR;

namespace CatalogService.UseCases.ProductImageUseCases.Commands.DeleteProductImageById;

public class DeleteProductImagesByIdCommand : IRequest
{
    public List<int> ProductImageIdList { get; set; }
}
