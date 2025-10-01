using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_boss_comments")]
public class SupervisorComment
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("card_id")]
    public int DeclarationId { get; set; }
    
    [ForeignKey(nameof(DeclarationId))]
    public Declaration Declaration { get; set; }
    
    [Column("pers_id")]
    public int SupervisorId { get; set; }
    
    [ForeignKey(nameof(SupervisorId))]
    public User Supervisor { get; set; }
    
    [Column("date")]
    public DateTime CommentDate { get; set; }
    
    [Column("text")]
    public string Comment { get; set; }
}