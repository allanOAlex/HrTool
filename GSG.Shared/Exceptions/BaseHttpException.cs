using System.Net;
using System.Text.Json;

namespace GSG.Shared.Exceptions;

using Microsoft.AspNetCore.Http;

public abstract class BaseHttpException : Exception
{
    private readonly int _statusCode;
    protected abstract object Body { get; }

    public BaseHttpException(HttpStatusCode statusCode, string message) : base(message: message)
    {
        _statusCode = (int)statusCode;
    }

    public async Task WriteResponse(HttpResponse response)
    {
        response.ContentType = "application/json";

        response.StatusCode = _statusCode;

        var result = JsonSerializer.Serialize(new ReponseBody
        {
            Success = false,
            ReponseCode = _statusCode,
            Message = Message,
            Body = Body
        });
        await response.WriteAsync(result);
    }
}