using MediatR;

namespace OrderService.UseCases.UserUseCases.Commands.EditUser;

public class EditUserCommand : IRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public int OriginalUserId { get; set; }
}
