using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_history")]
public class DeclarationAction
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [Column("rk_id")]
    public int DeclarationId { get; set; }
    
    [ForeignKey(nameof(DeclarationId))]
    public Declaration Declaration { get; set; }
    
    [Column("action")]
    public string Action { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
}