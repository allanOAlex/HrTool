using GSG.Logging;
using GSG.Repository;

namespace GSG.API.Middleware;

public class LoggingUserMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingUserMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        this.next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var httpContext = context.RequestServices.GetService<IHttpContextAccessor>();
        string userName = "anonymous";
        if (httpContext.HttpContext.User.Identity.IsAuthenticated)
        {
            var idContext = context.RequestServices.GetService<IIdentityContext>();
            userName = idContext.UserName;
        }
        using (_logger.BeginScope(new Dictionary<string, object> { ["UserId"] = userName }))
        {
            try
            {
                _logger.LogInformation("test");

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}