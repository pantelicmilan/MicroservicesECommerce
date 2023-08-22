using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryByName;

public class DeleteSubCategoryByNameCommandHandler : IRequestHandler<DeleteSubCategoryByNameCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DeleteSubCategoryByNameCommandHandler(
        ISubCategoryRepository subCategoryRepository,
        IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        _subCategoryRepository = subCategoryRepository;
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Handle(DeleteSubCategoryByNameCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetSubcategoryByName(request.SubCategoryName);
        if (subCategory == null) throw new Exception("Sub category not found");
        foreach(var product in subCategory.Products)
        {
            foreach(var productImage in product.ProductImages)
            {
                DeleteImageFileHandler.DeleteImageFileByImageLink(productImage.ImageUrl, _webHostEnvironment);
            }
        }
        _subCategoryRepository.DeleteSubcategory(subCategory);
        await _unitOfWork.SaveChangesAsync();
    }
}
