using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_themes")]
public class DeclarationTheme
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("type_id")]
    public int TypeId { get; set; }
    
    [ForeignKey(nameof(TypeId))]
    public DeclarationType Type { get; set; }
}