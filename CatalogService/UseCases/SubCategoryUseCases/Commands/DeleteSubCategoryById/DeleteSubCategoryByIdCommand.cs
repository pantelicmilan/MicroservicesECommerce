using MediatR;

namespace CatalogService.UseCases.SubCategoryUseCases.Commands.DeleteSubCategoryById;

public class DeleteSubCategoryByIdCommand : IRequest
{
    public int Id { get; set; }
}
