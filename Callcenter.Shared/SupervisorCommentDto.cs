namespace Callcenter.Shared;

public class SupervisorCommentDto
{
    public UserDto Supervisor { get; set; }
    
    public DateTime CommentDate { get; set; }
    
    public string Comment { get; set; }
}