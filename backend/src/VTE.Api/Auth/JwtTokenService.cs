using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VTE.Domain;
using VTE.Domain.Identity;

namespace VTE.Api.Auth;

public class JwtTokenService
{
    private readonly JwtOptions _opts;
    private readonly UserManager<ApplicationUser> _users;

    public JwtTokenService(JwtOptions opts, UserManager<ApplicationUser> users)
    {
        _opts = opts;
        _users = users;
    }

    public async Task<string> CreateTokenAsync(ApplicationUser user, int? stationId)
    {
        var roles = await _users.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        if (stationId.HasValue)
        {
            claims.Add(new Claim(VteClaims.StationId, stationId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_opts.AccessTokenMinutes),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "VTE";
    public string Audience { get; set; } = "VTE";
    public int AccessTokenMinutes { get; set; } = 60;
}
