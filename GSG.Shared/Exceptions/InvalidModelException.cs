namespace GSG.Shared.Exceptions;

public class InvalidModelException : Exception
{
    public InvalidModelException(string message = null) : base(message: message)
    {
    }
}