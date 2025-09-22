using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_files")]
public class DeclarationFile
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("date_added")]
    public DateTime DateAdded { get; set; }
    
    [Column("added_user_id")]
    public int AddedUserId { get; set; }
    
    [Column("card_id")]
    public int DeclarationId { get; set; }
    
    [ForeignKey(nameof(DeclarationId))]
    public Declaration Declaration { get; set; }
    
    [Column("name_real")]
    public string NameReal { get; set; }
}