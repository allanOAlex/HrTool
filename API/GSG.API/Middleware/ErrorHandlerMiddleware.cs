using GSG.Shared;
using GSG.Shared.Exceptions;

namespace GSG.API.Middleware;

using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseHttpException error)
        {
            var response = context.Response;
            await error.WriteResponse(response);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var body = new ResponseBody<string>
            {
                ReponseCode = 500,
                Success = false,
                Message = "Exception",
                Body = error.Message
            };

            var result = JsonSerializer.Serialize(body);
            await response.WriteAsync(result);
        }
    }
}