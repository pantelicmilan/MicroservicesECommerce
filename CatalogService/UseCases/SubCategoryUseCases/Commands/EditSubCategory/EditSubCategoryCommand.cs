using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.EditSubCategory;

public class EditSubCategoryCommand : IRequest
{
    public string SubCategoryName { get; set; }
    public int SubCategoryId { get; set; }
}
