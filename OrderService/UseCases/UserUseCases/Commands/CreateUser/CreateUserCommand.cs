using MediatR;

namespace OrderService.UseCases.UserUseCases.Commands.CreateUser;

public class CreateUserCommand : IRequest
{
    public int OriginalUserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}
