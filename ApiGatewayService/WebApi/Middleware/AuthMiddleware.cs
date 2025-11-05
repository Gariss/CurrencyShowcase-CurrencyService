using ApiGatewayService.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace ApiGatewayService.WebApi.Middleware;

public class AuthMiddleware(
        RequestDelegate next,
        IWhitelistService whitelistService,
        ILogger<AuthMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IWhitelistService _whitelistService = whitelistService;
    private readonly ILogger<AuthMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;

        if (path.StartsWithSegments("/api/users/login") && method == "POST")
        {
            await HandleLogin(context);
            return;
        }

        if (path.StartsWithSegments("/api/users/logout") && method == "POST")
        {
            await HandleLogout(context);
            return;
        }

        if (!IsPublicEndpoint(context.Request.Path) && context.User.Identity?.IsAuthenticated == true)
        {
            if (!await CheckWhitelist(context))
            {
                return;
            }
            SetUsedIdHeader(context);
        }

        await _next(context);
    }

    // forwarding user id via header
    private void SetUsedIdHeader(HttpContext context)
    {
        var userId = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                     context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            context.Request.Headers["X-User-Id"] = userId;
            _logger.LogDebug("Added X-User-Id header: {userId}", userId);
        }
    }

    private async Task HandleLogin(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status200OK &&
            context.Response.Headers.TryGetValue("Authorization", out var authHeader) &&
            authHeader.ToString().StartsWith("Bearer "))
        {
            var token = authHeader.ToString().Substring("Bearer ".Length).Trim();

            var (jti, expiry) = ExtractJtiAndExpiry(token);
            if (!string.IsNullOrEmpty(jti))
            {
                _whitelistService.Add(jti, expiry);
                _logger.LogInformation("JTI {jti} added to whitelist after login", jti);
            }
        }
    }

    private async Task HandleLogout(HttpContext context)
    {
        var token = ExtractTokenFromRequest(context);
        if (string.IsNullOrEmpty(token))
        {
            await WriteProblemDetails(context,
                StatusCodes.Status400BadRequest,
                "Missing token",
                "Authorization header with Bearer token is required for logout");
            return;
        }

        var (jti, expiry) = ExtractJtiAndExpiry(token);
        if (string.IsNullOrEmpty(jti))
        {
            await WriteProblemDetails(context,
                StatusCodes.Status400BadRequest,
                "Invalid token",
                "Token is missing JTI (JWT ID) claim");
            return;
        }

        _whitelistService.Remove(jti);

        _logger.LogInformation("User logged out, JTI {jti} removed from whitelist", jti);

        context.Response.StatusCode = StatusCodes.Status200OK;
    }

    private async Task<bool> CheckWhitelist(HttpContext context)
    {
        var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (string.IsNullOrEmpty(jti))
        {
            await WriteProblemDetails(context,
                StatusCodes.Status401Unauthorized,
                "Invalid token",
                "Token is not identified");
            return false;
        }

        if (!_whitelistService.IsActive(jti))
        {
            await WriteProblemDetails(context,
                StatusCodes.Status401Unauthorized,
                "Token is not active",
                "Token is not registered or session has ended");
            return false;
        }

        return true;
    }

    private async Task WriteProblemDetails(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = context.Request.Path
        };

        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        problem.Extensions["traceId"] = traceId;

        await context.Response.WriteAsJsonAsync(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private static bool IsPublicEndpoint(PathString path)
    {
        var publicPaths = new[]
        {
            "/api/users/login",
            "/api/users/register",
            "/swagger"
        };

        return publicPaths.Any(p => path.StartsWithSegments(p));
    }

    private static string? ExtractTokenFromRequest(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;

        return authHeader.Substring("Bearer ".Length).Trim();
    }

    private static (string? jti, DateTime expiry) ExtractJtiAndExpiry(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            return (jti, jwtToken.ValidTo);
        }
        catch
        {
            return (null, DateTime.MinValue);
        }
    }
}