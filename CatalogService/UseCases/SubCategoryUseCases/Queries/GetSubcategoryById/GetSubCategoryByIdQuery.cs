using CatalogService.Entitites;
using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Queries.GetSubcategoryById;

public class GetSubCategoryByIdQuery : IRequest<Subcategory>
{
    public int Id { get; set; }
}
