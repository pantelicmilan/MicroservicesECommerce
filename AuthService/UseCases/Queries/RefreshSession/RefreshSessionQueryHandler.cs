using AuthService.Services.Abstractions;
using MediatR;

namespace AuthService.Users.Queries.RefreshSession;

public class RefreshSessionQueryHandler : IRequestHandler<RefreshSessionQuery, string>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtTokenHandler _jwtTokenHandler;
    public RefreshSessionQueryHandler(IHttpContextAccessor contextAccessor, IJwtTokenHandler jwtTokenHandler)
    {
        _contextAccessor = contextAccessor;
        _jwtTokenHandler = jwtTokenHandler;
    }

    public async Task<string> Handle(RefreshSessionQuery request, CancellationToken cancellationToken)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new Exception("HttpContext not found!");
        }
        string currentRefreshTokenOfClient = null;
        httpContext.Request.Cookies.TryGetValue("refreshToken", out currentRefreshTokenOfClient);
        if(currentRefreshTokenOfClient == null)
        {
            throw new Exception("Refresh token not found!");
        }

        Console.WriteLine(currentRefreshTokenOfClient);

        var newAccessToken = await _jwtTokenHandler.RefreshSessionWithRefreshToken(currentRefreshTokenOfClient);
        if (newAccessToken == null) throw new Exception("New Jwt access token is null");

        return newAccessToken;
    }
}
