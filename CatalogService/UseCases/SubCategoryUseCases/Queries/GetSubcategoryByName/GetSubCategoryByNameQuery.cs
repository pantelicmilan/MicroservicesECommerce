using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryByName;

public class GetSubCategoryByNameQuery : IRequest<Subcategory>
{
    public string SubcategoryName { get; set; }
}
