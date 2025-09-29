using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Models.Reports;

public class PgFormGroupByMoModel
{
    [Column("region_id")]
    public int? RegionId { get; set; } 
    
    [Column("region_name")]
    public string? RegionName { get; set; }
    
    [Column("mo_code")]
    public string? MoCode { get; set; }
    
    [Column("mo_name")]
    public string? MoName { get; set; }
    
    [Column("obr_theme")]
    public string? ObrTheme { get; set; }
    
    [Column("ust_pis")]
    public int UstPis { get; set; }
}