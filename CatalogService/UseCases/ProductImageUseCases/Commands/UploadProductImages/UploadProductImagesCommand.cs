using MediatR;

namespace CatalogService.UseCases.ProductImageUseCases.Commands.UploadProductImages;

public class UploadProductImagesCommand : IRequest
{
    public List<IFormFile> Images { get; set; }
    public int ProductId { get; set; }
}
