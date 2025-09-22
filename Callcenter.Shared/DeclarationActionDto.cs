namespace Callcenter.Shared;

public class DeclarationActionDto
{
    public int Id { get; set; }
    
    public UserDto User { get; set; }
    
    public string Action { get; set; }
    
    public DateTime Date { get; set; }
}