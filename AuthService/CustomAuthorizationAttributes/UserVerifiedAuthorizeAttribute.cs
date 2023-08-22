using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AuthService.CustomAuthorizationAttributes;

public class UserVerifiedAuthorizeAttribute : AuthorizeAttribute
{
    private const string POLICY_PREFIX = "mail-verified";
    public string Roles = "";
    public UserVerifiedAuthorizeAttribute(string verified)
    {
        Verified = verified;
    }
    public string Verified
    {
        get => Policy.Substring(POLICY_PREFIX.Length);
        set => Policy = $"{POLICY_PREFIX}{value}";
    }
}

