namespace Callcenter.Shared;

public class GetRkListDto
{
    public int Id { get; set; }
    
    public string RkNumber { get; set; }
    
    public bool IsBad { get; set; }
    
    public DateTime DateReg { get; set; }
    
    public string ObrTheme { get; set; }
    
    public string Status { get; set; }
    
    public int? StatusId { get; set; }
}