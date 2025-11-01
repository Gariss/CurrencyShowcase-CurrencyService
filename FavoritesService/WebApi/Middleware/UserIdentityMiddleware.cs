namespace FavoritesService.WebApi.Middleware;

public class UserIdentityMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdHeader))
        {
            var userId = userIdHeader.ToString();
            context.Items["UserId"] = userId;
        }

        await _next(context);
    }
}