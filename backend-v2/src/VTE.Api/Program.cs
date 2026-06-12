using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VTE.Api.Auth;
using VTE.Api.Seed;
using VTE.Api.Tenancy;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

var builder = WebApplication.CreateBuilder(args);

// -------- Config: JWT + Cookie --------
var jwtOpts = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOpts);
builder.Services.AddSingleton<JwtTokenService>();

// -------- EF Core + Identity --------
builder.Services.AddDbContext<VteDbContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.CommandTimeout(120))); // 30s default is too tight for the
                                          // first-run vehicles search across
                                          // 66k relations until plans warm up.

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
{
    // Relaxed for the dev environment — operators are created by admins and
    // there's no email verification step. Tighten before production.
    o.Password.RequireDigit            = false;
    o.Password.RequiredLength          = 6;
    o.Password.RequireNonAlphanumeric  = false;
    o.Password.RequireUppercase        = false;
    o.Password.RequireLowercase        = false;
    o.User.RequireUniqueEmail          = false;
})
.AddEntityFrameworkStores<VteDbContext>()
.AddDefaultTokenProviders();

// -------- Tenancy --------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// -------- Pricing (Phase 3: auto-debt creation) --------
builder.Services.AddScoped<VTE.Infrastructure.Pricing.IPricingEvaluator, VTE.Infrastructure.Pricing.PricingEvaluator>();
builder.Services.AddScoped<VTE.Infrastructure.Pricing.IDebtService, VTE.Infrastructure.Pricing.DebtService>();

// -------- Authentication: BOTH JWT (default) + Cookie --------
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.RequireHttpsMetadata = false;
    opts.SaveToken = true;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOpts.Issuer,
        ValidAudience = jwtOpts.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Secret)),
        ClockSkew = TimeSpan.FromMinutes(2),
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
{
    opts.Cookie.Name = builder.Configuration["Cookie:Name"] ?? "vte.v2.auth";
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SameSite = SameSiteMode.Lax;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = TimeSpan.FromHours(
        int.TryParse(builder.Configuration["Cookie:ExpireHours"], out var h) ? h : 8);
    // For an API: never redirect to a login page — return 401 instead.
    opts.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    opts.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// -------- Authorization: accept either JWT or cookie by default --------
builder.Services.AddAuthorization(opts =>
{
    opts.DefaultPolicy = new AuthorizationPolicyBuilder(
            JwtBearerDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// -------- CORS for the Vue frontend --------
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins(allowedOrigins)
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()));

// -------- MVC + Swagger --------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VTE API v2", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT bearer. Paste: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// -------- Pipeline --------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VTE API v2");
    c.RoutePrefix = "swagger";
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// -------- SPA hosting --------
// In production the Vue build is copied into wwwroot (deploy/build-release.ps1), and the
// API serves it directly: same origin, so no CORS or separate static host needed.
// Client-side routes (e.g. /clients/42) fall back to index.html. In dev there is no
// wwwroot build — Vite serves the frontend — so "/" keeps redirecting to Swagger.
var webRoot = app.Environment.WebRootPath
    ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
if (File.Exists(Path.Combine(webRoot, "index.html")))
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
}
else
{
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

// -------- Migrate + Seed on startup --------
// Wrapped in try/catch so a transient DB outage (e.g. LocalDB not started) doesn't
// prevent the API from booting. DB-bound endpoints will surface the real error on
// their first call; non-DB routes (Swagger, /) stay healthy.
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
        await db.Database.MigrateAsync();
    }
    await DataSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    logger.LogError(ex, "Startup migration/seed failed — continuing anyway so the host can serve requests once the DB is reachable.");
}

app.Run();
