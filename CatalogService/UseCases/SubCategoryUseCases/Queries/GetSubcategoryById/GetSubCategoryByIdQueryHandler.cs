using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryById;

public class GetSubCategoryByIdQueryHandler : IRequestHandler<GetSubCategoryByIdQuery, Subcategory>
{
    private readonly ISubCategoryRepository _subCategoryRepository;

    public GetSubCategoryByIdQueryHandler(ISubCategoryRepository subCategoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
    }

    public async Task<Subcategory> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetSubcategoryById(request.Id);
        if (subCategory == null) throw new Exception("Subcategory not found");
        return subCategory;
    }
}
