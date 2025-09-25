namespace Callcenter.Shared;

public class DeclarationActionDto
{
    public int Id { get; set; }
    
    public string UserFullName { get; set; }
    
    public string UserGroupName { get; set; }
    
    public string Action { get; set; }
    
    public DateTime Date { get; set; }
}