using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Auth;
using VTE.Domain.Identity;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly JwtTokenService _jwt;
    private readonly VteDbContext _db;

    public AuthController(
        UserManager<ApplicationUser> users,
        SignInManager<ApplicationUser> signIn,
        JwtTokenService jwt,
        VteDbContext db)
    {
        _users = users;
        _signIn = signIn;
        _jwt = jwt;
        _db = db;
    }

    public record LoginRequest(string UserName, string Password);
    public record LoginResponse(string Token, string UserName, string[] Roles, int? StationId);

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest req)
    {
        var user = await _users.FindByNameAsync(req.UserName);
        if (user is null) return Unauthorized();

        var ok = await _signIn.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
        if (!ok.Succeeded) return Unauthorized();

        // Resolve operator's StationId (Administrators have no Operator row → null)
        var stationId = await _db.Operators
            .Where(o => o.UserId == user.Id)
            .Select(o => (int?)o.StationId)
            .FirstOrDefaultAsync();

        var roles = (await _users.GetRolesAsync(user)).ToArray();
        var token = await _jwt.CreateTokenAsync(user, stationId);

        return Ok(new LoginResponse(token, user.UserName ?? string.Empty, roles, stationId));
    }
}
