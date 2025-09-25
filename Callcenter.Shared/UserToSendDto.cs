namespace Callcenter.Shared;

public class UserToSendDto
{
    public int Id { get; set; }
    
    public int GroupId { get; set; }
    
    public string GroupName { get; set; }
    
    public int OrgId { get; set; }
    
    public string OrgName { get; set; }
    
    public string FullName { get; set; }
}