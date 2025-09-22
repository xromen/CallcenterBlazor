using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("f003_document", Schema = "registers")]
public class F003Document
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("d_end")]
    public DateOnly? DateEnd { get; set; }
}