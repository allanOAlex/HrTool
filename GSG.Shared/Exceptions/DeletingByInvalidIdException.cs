namespace GSG.Shared.Exceptions;

public class DeletingByInvalidIdException : Exception
{
    public DeletingByInvalidIdException(string message = null) : base(message: message)
    {
    }
}