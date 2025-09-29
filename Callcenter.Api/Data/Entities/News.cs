using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_news")]
public class News
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("text")]
    public string Text { get; set; }
    
    [Column("date")]
    public DateOnly Date { get; set; }
    
    [Column("user_create")]
    public int CreatorUserId { get; set; }
    
    [ForeignKey(nameof(CreatorUserId))]
    public User? CreatedBy { get; set; }

    public List<Organisation> Organisations { get; set; } = new();
}