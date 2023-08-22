using CatalogService.Repositories.Abstractions;
using MediatR;
using CatalogService.Entitites;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.CreateSubCategory;

public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
    {
        _subCategoryRepository = subCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ReferenceCategoryId == null || request.SubCategoryName == null) throw new Exception("Invalid input");
        var subCategoryCheck = await _subCategoryRepository.GetSubcategoryByName(request.SubCategoryName);
        if (subCategoryCheck != null) throw new Exception("Subcategory already exist");
        _subCategoryRepository.CreateSubCategory(
            new Subcategory { Name = request.SubCategoryName, CategoryId = request.ReferenceCategoryId }
            );
        await _unitOfWork.SaveChangesAsync();
    }
}
