using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Models;

public class GetRkListModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("rk_number")]
    public string RkNumber { get; set; }
    
    [Column("is_bad")]
    public int IsBad { get; set; }
    
    [Column("datereg")]
    public DateTime DateReg { get; set; }
    
    [Column("obr_theme")]
    public string ObrTheme { get; set; }
    
    [Column("statusik")]
    public string Status { get; set; }
    
    [Column("statusik_id")]
    public int? StatusId { get; set; }
    
    [Column("total_count")]
    public long TotalItems { get; set; }
    
    [Column("send_answer_count")]
    public long SendAnswerCount { get; set; }
    
    [Column("needs_rework_count")]
    public long NeedReworkCount { get; set; }
    
    [Column("smo_redirect_count")]
    public long SmoRedirectCount { get; set; }
}