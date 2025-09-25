namespace Callcenter.Shared;

public class QuestionCreateRequest
{
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Answer { get; set; }
}