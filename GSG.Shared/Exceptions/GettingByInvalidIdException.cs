namespace GSG.Shared.Exceptions;

public class GettingByInvalidIdException : Exception
{
    public GettingByInvalidIdException(string message = null) : base(message: message)
    {
    }
}
