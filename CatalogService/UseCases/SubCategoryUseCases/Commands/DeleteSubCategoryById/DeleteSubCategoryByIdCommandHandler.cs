using CatalogService.Repositories.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryById;

public class DeleteSubCategoryByIdCommandHandler : IRequestHandler<DeleteSubCategoryByIdCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DeleteSubCategoryByIdCommandHandler(
        ISubCategoryRepository subCategoryRepository, 
        IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        _subCategoryRepository = subCategoryRepository;
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Handle(DeleteSubCategoryByIdCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetSubcategoryById(request.Id);
        if (subCategory == null) throw new Exception("Sub category not found");
        foreach (var product in subCategory.Products)
        {
            foreach (var productImage in product.ProductImages)
            {
                DeleteImageFileHandler.DeleteImageFileByImageLink(productImage.ImageUrl, _webHostEnvironment);
            }
        }
        _subCategoryRepository.DeleteSubcategory(subCategory);
        await _unitOfWork.SaveChangesAsync();
    }
}
