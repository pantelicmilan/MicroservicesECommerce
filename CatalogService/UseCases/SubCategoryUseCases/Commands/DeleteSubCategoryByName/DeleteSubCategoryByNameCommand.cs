using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryByName;

public class DeleteSubCategoryByNameCommand : IRequest
{
    public string SubCategoryName { get; set; }
}
