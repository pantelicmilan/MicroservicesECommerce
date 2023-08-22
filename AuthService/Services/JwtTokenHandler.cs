using AuthService.Entities;
using AuthService.Repositories.Abstractions;
using AuthService.Services.Abstractions;
using JwtAuthLibrary;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services;

public class JwtTokenHandler : IJwtTokenHandler
{
    private readonly string REFRESH_TOKEN_SECRET_KEY = "jwt-auth-refresh-key";
    private readonly string ACCESS_TOKEN_SECRET_KEY = "jwt-auth-secret-key";
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public JwtTokenHandler(
        IConfiguration configuration, 
        IUserRepository userRepository, 
        IHttpContextAccessor contextAccessor)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _contextAccessor = contextAccessor;
    }

    public string GenerateJwtAccessToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim("username", user.Username),
                new Claim("acc-role", user.Role.ToString()),
                new Claim("mail-verified", user.MailVerified.ToString()),
                new Claim("id", user.Id.ToString())
            };
            string privateKey = _configuration.GetConnectionString(ACCESS_TOKEN_SECRET_KEY);
            if (privateKey == null)
            {
                throw new Exception("Internal error with Private Key!");
            }
            int validityInHours = 4;
            var token = JwtTokenFactory(claims, privateKey, validityInHours);
            return token;
        }
        catch
        {
            throw new Exception("Error in generating token!");
        }
    }

    public string GenerateJwtRefreshToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
            };
            var validityInHours = 167;
            string privateKey = _configuration.GetConnectionString(REFRESH_TOKEN_SECRET_KEY);
            if (privateKey == null) throw new Exception("Private key for refresh token not found");
            string refreshToken = JwtTokenFactory(claims, privateKey, validityInHours);
            return refreshToken;
        }
        catch
        {
            throw new Exception("Error in generating token!");

        }
    }

    public async Task<string> RefreshSessionWithRefreshToken(string refreshToken)
    {
        var secretKey = _configuration[REFRESH_TOKEN_SECRET_KEY];
        if (secretKey == null) throw new Exception("Problem in internal secret key storage!");
        List<Claim> claims = SharedAuthHandler.GetListOfClaimsIfTokenValid(refreshToken);
        if (claims == null) throw new Exception("Claims invalid");
        Claim userIdClaim = claims.Find(c => c.Type == "id");

        if (userIdClaim == null) throw new Exception("You do not have a valid claim inside your token");
        int? userId = null;
        if (int.TryParse(userIdClaim.Value, out int result))
        {
            userId = result;
        }
        else
        {
            throw new Exception("Error in value in claim");
        }
        if (userId == null) throw new Exception("Exception");
        var userFromRefreshToken = await _userRepository.GetUserByUserId((int)userId);
        var newAccessToken = GenerateJwtAccessToken(userFromRefreshToken);
        var newRefreshToken = GenerateJwtRefreshToken(userFromRefreshToken);

        if (newRefreshToken == null) throw new Exception("Error with creating refresh token!");
        if (newAccessToken == null) throw new Exception("Error with creating access token!");

        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null) throw new Exception("Error with access in HttpContext");

        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken);
        return newAccessToken;
    }


    private string JwtTokenFactory(List<Claim> claims, string privateKey, int validityInHours)
    {
        if (privateKey == null)
        {
            throw new Exception("Internal error with Private Key!");
        }
        var tokenExpiryTimestamp = DateTime.Now.AddHours(validityInHours);
        var tokenKey = Encoding.ASCII.GetBytes(privateKey);
        if (claims == null) throw new Exception("Claims error, you do not have a claims");
        var claimsIdentity = new ClaimsIdentity(claims);
        var signingCredentials = new SigningCredentials(
                   new SymmetricSecurityKey(tokenKey),
                   SecurityAlgorithms.HmacSha256Signature);
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = signingCredentials,
            Subject = claimsIdentity,
            Expires = tokenExpiryTimestamp,
            Audience = "all",
            Issuer = "auth_microservice"
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var refreshToken = jwtSecurityTokenHandler.WriteToken(securityToken);
        return refreshToken;
    }
}
