namespace Callcenter.Web.Models;

public class DeclarationListDto
{
    public int Id { get; set; }
    
    public string Number { get; set; }
    
    public bool IsBad { get; set; }
    
    public DateTime DateReg { get; set; }
    
    public string Theme { get; set; }
    
    public string Status { get; set; }
    
    public int? StatusId { get; set; }
}