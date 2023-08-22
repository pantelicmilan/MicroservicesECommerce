using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest
{
    public string CategoryName { get; set; }
}
