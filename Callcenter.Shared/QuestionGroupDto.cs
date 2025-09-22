namespace Callcenter.Shared;

public class QuestionGroupDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<QuestionDto> Questions { get; set; } = new();
}