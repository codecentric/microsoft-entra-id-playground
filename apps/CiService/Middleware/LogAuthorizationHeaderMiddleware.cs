using Microsoft.Net.Http.Headers;

namespace CiService.Middleware;

public class LogAuthorizationHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogAuthorizationHeaderMiddleware> _logger;

    public LogAuthorizationHeaderMiddleware(RequestDelegate next, ILogger<LogAuthorizationHeaderMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeaderValue);
            _logger.LogTrace("Authorization Header: {AuthorizationHeader}", authorizationHeaderValue.FirstOrDefault());
        }

        await _next(context);
    }
}

// Extension method to make it easy to add the middleware to the pipeline
public static class LogHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseLogAuthorizationHeader(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogAuthorizationHeaderMiddleware>();
    }
}