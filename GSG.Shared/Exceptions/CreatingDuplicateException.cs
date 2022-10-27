namespace GSG.Shared.Exceptions;

public class CreatingDuplicateException : Exception
{
    public CreatingDuplicateException(string message = null) : base(message: message)
    {
    }
}
