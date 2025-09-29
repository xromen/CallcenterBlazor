using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Models.Reports;

public class JustifiedComplaintsFormModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("mo_name")]
    public string? MoName { get; set; }
    
    [Column("form_type")]
    public string? FormType { get; set; }
    
    [Column("form_name")]
    public string? FormName { get; set; }
    
    [Column("date_reg")]
    public DateTime RegistrationDate { get; set; }
    
    [Column("obr_theme")]
    public string? Theme { get; set; }
    
    [Column("content")]
    public string? Content { get; set; }
    
    [Column("work_done")]
    public string? WorkDone { get; set; }
    
    [Column("vid_mp_name")]
    public string? TypeMpName { get; set; }
}