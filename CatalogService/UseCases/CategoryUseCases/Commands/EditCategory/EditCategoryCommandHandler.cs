using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Commands.EditCategory;

public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.CategoryName == null || request.CategoryId == null) 
            throw new Exception("Command not have enough information");
        var category = await _categoryRepository.GetCategoryById(request.CategoryId);
        if (category == null) throw new Exception("Category not found");
        category.Name = request.CategoryName;
        await _unitOfWork.SaveChangesAsync();
    }

}
