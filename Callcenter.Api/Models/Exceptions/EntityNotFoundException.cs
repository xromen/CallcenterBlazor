namespace Callcenter.Api.Models.Exceptions;

public class EntityNotFoundException : Exception
{
    public long Id { get; set; }
    
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}