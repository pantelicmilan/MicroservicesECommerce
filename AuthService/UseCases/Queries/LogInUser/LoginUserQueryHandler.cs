using AuthService.Exceptions;
using AuthService.Repositories.Abstractions;
using AuthService.Services.Abstractions;
using MediatR;
using System.Security.Authentication;

namespace AuthService.Users.Queries.LogInUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtTokenHandler _jwtTokenHandler;

    public LoginUserQueryHandler(
        IUserRepository userRepository, 
        IHttpContextAccessor httpContext, 
        IJwtTokenHandler jwtTokenHandler)
    {
        _userRepository = userRepository;
        _contextAccessor = httpContext;
        _jwtTokenHandler = jwtTokenHandler;
    }

    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetUserByUsernameAsync(request.UserName);
        if(userFromDb == null)
        {
            throw new NotFoundException("Username not found!");
        }

        if(!BCrypt.Net.BCrypt.Verify(request.Password, userFromDb.Password))
        {
            throw new AuthenticationException("Bad password!");
        }

        string jwtToken = _jwtTokenHandler.GenerateJwtAccessToken(userFromDb);
        if(jwtToken == null)
        {
            throw new Exception("Error in generating token!");
        }
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null) throw new Exception("Error with accessing in HttpContext");
        
        httpContext.Response.Cookies.Append("refreshToken", _jwtTokenHandler.GenerateJwtRefreshToken(userFromDb));
        return jwtToken;
    }

}
