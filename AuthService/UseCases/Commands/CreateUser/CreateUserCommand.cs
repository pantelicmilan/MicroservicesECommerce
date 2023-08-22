using MediatR;

namespace AuthService.Users.Commands.CreateUser;

public sealed record CreateUserCommand : IRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}
