using MediatR;

namespace AuthService.Users.Queries.LogInUser;

public class LoginUserQuery: IRequest<string>
{
    public string UserName { get; set;}
    public string Password { get; set;}
}
