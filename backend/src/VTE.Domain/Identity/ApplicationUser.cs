using Microsoft.AspNetCore.Identity;

namespace VTE.Domain.Identity;

// Identity user for VTE — extends the standard AspNetUsers row.
// Per-tenant binding lives on the Operator entity (1:1 with ApplicationUser via UserId).
// Administrators have NO Operator row (they're cross-tenant).
public class ApplicationUser : IdentityUser
{
}
