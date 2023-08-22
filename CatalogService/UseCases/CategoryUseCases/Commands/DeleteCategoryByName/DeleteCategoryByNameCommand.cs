using MediatR;

namespace CatalogService.UseCases.CategoryUseCases.Commands.DeleteCategoryByName;

public class DeleteCategoryByNameCommand : IRequest
{
    public string CategoryName { get; set; }
}
