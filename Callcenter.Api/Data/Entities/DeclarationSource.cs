using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_source")]
public class DeclarationSource
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}