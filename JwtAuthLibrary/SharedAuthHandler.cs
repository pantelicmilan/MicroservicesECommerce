using JwtAuthLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthLibrary;

public class SharedAuthHandler
{
    public const string JWT_SECURITY_KEY = "a51881797a3273e85f061e9a4ed52b07d0a95707efb696eeeb89f9cad25b8fc1";

    public static List<Claim> GetListOfClaimsIfTokenValid(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
            return claimsPrincipal.Claims.ToList();
        }
        catch
        {
            throw new Exception("You have a invalid refresh token!");
        }
    }

    public static int GetIdClaimIfJwtTokenValid(string jwtToken)
    {
        List<Claim> claims = GetListOfClaimsIfTokenValid(jwtToken);
        if (claims == null) throw new Exception("Claims not found in jwt Token!");

        var userIdClaim = claims.FirstOrDefault(c => c.Type == "id").Value;
        if (userIdClaim == null) throw new Exception("User id not found in jwt payload!");

        int userId = 0;
        if (userIdClaim.GetType() == typeof(string))
        {
            userId = Convert.ToInt32(userIdClaim);
        }
        if (userId == 0)
        {
            throw new Exception("Error with grabbing id!");
        }
        return userId;
    }

    public static string GetJwtTokenFromHttpContext(HttpContext httpContext)
    {
        if (httpContext == null) throw new Exception("Error in catching http context");
        var token = httpContext.Request.Headers["Authorization"].ToString().Replace("bearer ", "");
        if (token == null) throw new Exception("Jwt token not found!");
        if (IsJwtTokenValid(token))
        {
            return token;
        }
        throw new Exception("Jwt Token not valid!");
    }

    public static string GetClaimValueByClaimKey(HttpContext httpContext, string claimKey)
    {
        if (httpContext == null) throw new Exception("Error in catching http context");
        var token = httpContext.Request.Headers["Authorization"].ToString().Replace("bearer ", "");
        if (token == null) throw new Exception("Jwt token not found!");
        List<Claim> claims = GetListOfClaimsIfTokenValid(token);
        if (claims == null) throw new Exception("Claims not found in jwt Token!");
        var valueByClaimKey = claims.FirstOrDefault(c => c.Type == claimKey).Value;
        if (valueByClaimKey == null) throw new Exception("Claim key not found in jwt payload!");
        return valueByClaimKey;
    }

    public static bool IsJwtTokenValid(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }



}
