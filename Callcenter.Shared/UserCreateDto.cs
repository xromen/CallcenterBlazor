namespace Callcenter.Shared;

public class UserCreateDto
{
    public int? Id { get; set; }
    
    public int? GroupId { get; set; }
    
    public string? Login { get; set; }
    
    public string? Password { get; set; }
    
    public int? OrgId { get; set; }

    public bool IsEnabled { get; set; } = true;
    
    public string? FullName { get; set; }
    
    public int? SpLevel { get; set; }
}