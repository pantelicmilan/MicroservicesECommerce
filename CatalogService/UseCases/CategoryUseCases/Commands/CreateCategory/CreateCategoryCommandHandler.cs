using CatalogService.Repositories.Abstractions;
using MediatR;
using CatalogService.Entitites;

namespace CatalogService.UseCases.CategoryUseCases.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryWithSameName = await _categoryRepository.GetCategoryByCategoryName(request.CategoryName);
        if (categoryWithSameName != null) throw new Exception("Category already exist");

        _categoryRepository.CreateCategory(new Category { Name = request.CategoryName });
        await _unitOfWork.SaveChangesAsync();
    }
}
