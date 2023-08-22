using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Commands.EditCategory;

public class EditCategoryCommand : IRequest
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
