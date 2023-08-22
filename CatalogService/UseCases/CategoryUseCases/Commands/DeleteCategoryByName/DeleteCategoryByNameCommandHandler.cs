using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Commands.DeleteCategoryByName;

public class DeleteCategoryByNameCommandHandler : IRequestHandler<DeleteCategoryByNameCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DeleteCategoryByNameCommandHandler(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Handle(DeleteCategoryByNameCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByCategoryName(request.CategoryName);
        if (category == null) throw new Exception("Category not found");

        foreach(var subcat in category.Subcategories)
        {
            foreach(var prod in subcat.Products)
            {
                foreach(var prodImages in prod.ProductImages)
                {
                    DeleteImageFileHandler.DeleteImageFileByImageLink(prodImages.ImageUrl,_webHostEnvironment);
                }
            }
        }

        _categoryRepository.DeleteCategory(category);
        await _unitOfWork.SaveChangesAsync();

    }
}
