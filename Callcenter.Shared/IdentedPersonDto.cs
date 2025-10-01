namespace Callcenter.Shared;

public record IdentedPersonDto
{
    public int? InsuredSmoId { get; set; }
    
    public string? ResidenceAddress { get; set; }
    
    public string? InsuredEnp { get; set; }
    
    public string? IdentityDocType { get; set; }
    
    public string? IdentityDocSeries { get; set; }
    
    public string? IdentityDocNumber { get; set; }
}