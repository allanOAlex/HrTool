namespace GSG.Shared.Exceptions;

public class DeletingEntityWithExistingAttachmentsException : Exception
{
    public DeletingEntityWithExistingAttachmentsException(string message = null) : base(message: message)
    {
    }
}