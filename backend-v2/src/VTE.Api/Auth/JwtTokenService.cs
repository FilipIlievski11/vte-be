using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VTE.Domain.Identity;
using VTE.Api.Tenancy;

namespace VTE.Api.Auth;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "VTE-v2";
    public string Audience { get; set; } = "VTE-v2";
    public int AccessTokenMinutes { get; set; } = 480;
}

public class JwtTokenService
{
    private readonly JwtOptions _opts;

    public JwtTokenService(JwtOptions opts) => _opts = opts;

    public (string token, DateTime expiresAt) IssueToken(ApplicationUser user, IList<string> roles)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_opts.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        if (user.CompanyId.HasValue)
            claims.Add(new Claim(TenantContext.CompanyIdClaim, user.CompanyId.Value.ToString()));
        foreach (var r in roles)
            claims.Add(new Claim(ClaimTypes.Role, r));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public ClaimsPrincipal BuildPrincipalForCookie(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Id),
        };
        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        if (user.CompanyId.HasValue)
            claims.Add(new Claim(TenantContext.CompanyIdClaim, user.CompanyId.Value.ToString()));
        foreach (var r in roles)
            claims.Add(new Claim(ClaimTypes.Role, r));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies"));
    }
}
