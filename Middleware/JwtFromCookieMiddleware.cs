namespace DevSync.Middlewares;

public class JwtFromCookieMiddleware
{
    private readonly RequestDelegate _next;

    public JwtFromCookieMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization") &&
            context.Request.Cookies.TryGetValue("accessToken", out var token))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await _next(context);
    }
}
