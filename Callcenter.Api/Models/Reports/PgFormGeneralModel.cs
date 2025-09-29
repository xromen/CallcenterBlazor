using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Models.Reports;

public class PgFormGeneralModel
{
    [Column("zap_id")]
    public int Id { get; set; }
    
    [Column("type_id")]
    public int TypeId { get; set; }
    
    [Column("obr_theme")]
    public string Theme { get; set; }
    
    [Column("org_id")]
    public int OrganizationId { get; set; }
    
    [Column("ust_pis")]
    public int UstPis { get; set; }
    
    [Column("date_last_change")]
    public DateTime LastChange { get; set; } = DateTime.Now;
    
    [Column("is_real_jal")]
    public int IsRealJal { get; set; }
    
    [Column("date_reg")]
    public DateTime DateRegistration { get; set; }
}