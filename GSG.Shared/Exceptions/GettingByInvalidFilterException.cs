using System.Net;

namespace GSG.Shared.Exceptions;

public class GettingByInvalidFilterException<T> : BaseHttpException
{
    private readonly T _body;

    public GettingByInvalidFilterException(T body, string message = null) : base(HttpStatusCode.OK,
        message: message)
    {
        _body = body;
    }

    protected override object Body => _body;
}