using CatalogService.Entitites;
using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryByName;

public class GetSubCategoryByNameQueryHandler : IRequestHandler<GetSubCategoryByNameQuery, Subcategory>
{
    private readonly ISubCategoryRepository _subCategoryRepository;

    public GetSubCategoryByNameQueryHandler(ISubCategoryRepository subCategoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
    }

    public async Task<Subcategory> Handle(GetSubCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetSubcategoryByName(request.SubcategoryName);
        if (subCategory == null) throw new Exception("Subcategory not found");
        return subCategory;
    }
}
