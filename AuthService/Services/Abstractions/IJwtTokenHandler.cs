using AuthService.Entities;
using System.Security.Claims;

namespace AuthService.Services.Abstractions;

public interface IJwtTokenHandler
{
    public string GenerateJwtAccessToken(User user);
    public string GenerateJwtRefreshToken(User user);
    public Task<string> RefreshSessionWithRefreshToken(string refreshToken);
}
