using CatalogService.DataAccess;
using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CatalogService.UseCases.ProductImageUseCases.Commands.DeleteProductImageById;

public class DeleteProductImagesByIdCommandHandler : IRequestHandler<DeleteProductImagesByIdCommand>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public DeleteProductImagesByIdCommandHandler(
        IProductImageRepository productImageRepository, 
        IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        _productImageRepository = productImageRepository;      
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Handle(DeleteProductImagesByIdCommand request, CancellationToken cancellationToken)
    {
        if (request.ProductImageIdList.Count == 0) throw new Exception("You must specify items to delete");
        var matchingProductImagesList = await _productImageRepository.IfAllIdsExistReturnProductImageListAsync(request.ProductImageIdList);
        Console.WriteLine("Prosli smo matching product images!");
        if (matchingProductImagesList == null) throw new Exception("All id does not exist");

        foreach(var productImage in matchingProductImagesList)
        {
            Console.WriteLine("usli smo u for petlju");
            _productImageRepository.DeleteProductImage(productImage);
            DeleteImageFileHandler.DeleteImageFileByImageLink(productImage.ImageUrl, _webHostEnvironment);
        }
        await _unitOfWork.SaveChangesAsync();
    }
}
