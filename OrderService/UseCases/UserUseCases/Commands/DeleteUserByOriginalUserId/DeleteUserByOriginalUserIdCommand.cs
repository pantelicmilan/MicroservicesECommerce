using MediatR;

namespace OrderService.UseCases.UserUseCases.Commands.DeleteUserByOriginalUserId;

public class DeleteUserByOriginalUserIdCommand : IRequest
{
    public int OriginalUserId { get; set; }
}
