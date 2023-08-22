using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.CreateSubCategory;

public class CreateSubCategoryCommand : IRequest
{
    public int ReferenceCategoryId { get; set; }
    public string SubCategoryName { get; set; }
}
