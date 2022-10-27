using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace GSG.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var guid = Guid.NewGuid();

            using (_logger.BeginScope(new Dictionary<string, object> { ["Guid"] = guid }))
            {
                try
                {
                    _logger.LogInformation(context.Request.GetEncodedUrl());
                    context.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;
                        httpContext.Response.Headers.Add("X-Response-CorrelationId", new[] { guid.ToString() });

                        return Task.CompletedTask;
                    }, context);

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
}