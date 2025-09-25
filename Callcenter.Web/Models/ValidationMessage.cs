namespace Callcenter.Web.Models;

public class ValidationMessage
{
    public ValidationMessage(string message, bool isError)
    {
        Message = message;
        IsError = isError;
    }

    public string Message { get; set; }
    public bool IsError { get; }
    public bool IsWarning => !IsError;

    public static ValidationMessage Error(string message)
    {
        return new ValidationMessage(message, true);
    }

    public static ValidationMessage Warning(string message)
    {
        return new ValidationMessage(message, false);
    }
}