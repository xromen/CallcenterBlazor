namespace Callcenter.Shared;

public class NewsDto
{
    public int? Id { get; set; }
    
    public string Text { get; set; }
    
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    // public int CreatorUserId { get; set; }
    //
    // public UserDto CreatedBy { get; set; }

    public List<int> OrganisationIds { get; set; } = new();
}