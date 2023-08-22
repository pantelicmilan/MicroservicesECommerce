using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.EditSubCategory;

public class EditSubCategoryCommandHandler : IRequestHandler<EditSubCategoryCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public EditSubCategoryCommandHandler(ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
    {
        _subCategoryRepository = subCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentSubCategory = await _subCategoryRepository.GetSubcategoryById(request.SubCategoryId);
        if (currentSubCategory == null) throw new Exception("Sub category not exist");
        currentSubCategory.Name = request.SubCategoryName;
        await _unitOfWork.SaveChangesAsync();
    }
}
