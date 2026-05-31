using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly RoleManager<ApplicationRole> _roles;
    private readonly JwtTokenService _jwt;
    private readonly VteDbContext _db;

    public AuthController(
        UserManager<ApplicationUser> users,
        SignInManager<ApplicationUser> signIn,
        RoleManager<ApplicationRole> roles,
        JwtTokenService jwt,
        VteDbContext db)
    {
        _users = users;
        _signIn = signIn;
        _roles = roles;
        _jwt = jwt;
        _db = db;
    }

    /// <summary>Look up a Company's name from its Id, bypassing the tenant query filter.</summary>
    private async Task<string?> CompanyNameAsync(byte? companyId)
    {
        if (!companyId.HasValue) return null;
        return await _db.Companies.IgnoreQueryFilters().AsNoTracking()
            .Where(c => c.Id == companyId.Value)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();
    }

    /// <summary>Issues a JWT (and, when UseCookie=true, also sets the auth cookie).</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _users.FindByNameAsync(req.UserName);
        if (user == null || !user.IsActive)
            return Unauthorized(new { error = "Invalid credentials." });

        var check = await _signIn.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
        if (!check.Succeeded)
            return Unauthorized(new { error = check.IsLockedOut ? "Account is locked." : "Invalid credentials." });

        var roleNames = await _users.GetRolesAsync(user);
        var (token, expires) = _jwt.IssueToken(user, roleNames);

        if (req.UseCookie)
        {
            var principal = _jwt.BuildPrincipalForCookie(user, roleNames);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = expires });
        }

        var companyName = await CompanyNameAsync(user.CompanyId);
        return Ok(new LoginResponse(token, expires, user.Id, user.UserName!, user.FullName, user.CompanyId, companyName, roleNames.ToList()));
    }

    /// <summary>Signs the cookie session out (no-op for pure-JWT clients).</summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    /// <summary>Returns the current authenticated identity (works for both JWT and cookie callers).</summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<MeResponse>> Me()
    {
        var user = await _users.GetUserAsync(User);
        if (user == null) return Unauthorized();
        var roles = await _users.GetRolesAsync(user);
        var companyName = await CompanyNameAsync(user.CompanyId);
        return Ok(new MeResponse(user.Id, user.UserName!, user.Email, user.FullName, user.CompanyId, companyName, roles.ToList()));
    }

    /// <summary>Administrator-only: creates an Operator (or another Administrator) bound to a Company.</summary>
    [HttpPost("register")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<MeResponse>> Register([FromBody] RegisterRequest req)
    {
        if (!await _roles.RoleExistsAsync(req.Role))
            return BadRequest(new { error = $"Unknown role '{req.Role}'." });

        var existing = await _users.FindByNameAsync(req.UserName);
        if (existing != null)
            return Conflict(new { error = "User name already taken." });

        var user = new ApplicationUser
        {
            UserName = req.UserName,
            Email = req.Email,
            EmailConfirmed = true,
            FullName = req.FullName,
            CompanyId = req.CompanyId,
            IsActive = true,
        };

        var createResult = await _users.CreateAsync(user, req.Password);
        if (!createResult.Succeeded)
            return BadRequest(new { error = string.Join("; ", createResult.Errors.Select(e => e.Description)) });

        var addRoleResult = await _users.AddToRoleAsync(user, req.Role);
        if (!addRoleResult.Succeeded)
            return BadRequest(new { error = string.Join("; ", addRoleResult.Errors.Select(e => e.Description)) });

        var roles = await _users.GetRolesAsync(user);
        var companyName = await CompanyNameAsync(user.CompanyId);
        return Ok(new MeResponse(user.Id, user.UserName!, user.Email, user.FullName, user.CompanyId, companyName, roles.ToList()));
    }

    /// <summary>Lets an authenticated user set a new password — no current-password challenge.
    /// Identity is already proven by the JWT/cookie; we mint a reset token internally and apply it.</summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
    {
        var user = await _users.GetUserAsync(User);
        if (user == null) return Unauthorized();
        var token = await _users.GeneratePasswordResetTokenAsync(user);
        var result = await _users.ResetPasswordAsync(user, token, req.NewPassword);
        if (!result.Succeeded)
            return BadRequest(new { error = string.Join("; ", result.Errors.Select(e => e.Description)) });
        return NoContent();
    }

    /// <summary>Lets an authenticated user update their own display name and email.
    /// UserName (login) and CompanyId are intentionally NOT mutable here — only admins can change those.</summary>
    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<MeResponse>> UpdateProfile([FromBody] UpdateProfileRequest req)
    {
        var user = await _users.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (req.FullName != null)
            user.FullName = string.IsNullOrWhiteSpace(req.FullName) ? null : req.FullName.Trim();

        if (req.Email != null)
        {
            var email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim();
            user.Email = email;
            user.NormalizedEmail = email == null ? null : _users.NormalizeEmail(email);
            // Don't reset EmailConfirmed — self-service edit keeps the account usable.
        }

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { error = string.Join("; ", result.Errors.Select(e => e.Description)) });

        var roles = await _users.GetRolesAsync(user);
        var companyName = await CompanyNameAsync(user.CompanyId);
        return Ok(new MeResponse(user.Id, user.UserName!, user.Email, user.FullName, user.CompanyId, companyName, roles.ToList()));
    }

    public record ChangePasswordRequest(string NewPassword);
    public record UpdateProfileRequest(string? FullName, string? Email);
}
