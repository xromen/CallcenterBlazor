namespace Callcenter.Shared.Requests;

public class FindDeclarationsByFioRequest
{
    public string FirstName { get; set; }
    public string SecName { get; set; }
    public DateOnly BirthDate { get; set; }
}