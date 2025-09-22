using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Data.Entities;

[Table("f003_mo_document", Schema = "registers")]
[PrimaryKey(nameof(MoId), nameof(DocumentId))]
public class F003MoDocument
{
    [Column("idmo")]
    public Guid MoId { get; set; }
    
    [ForeignKey(nameof(MoId))]
    public F003Mo Mo { get; set; }
    
    [Column("iddoc")]
    public Guid DocumentId { get; set; }
    
    [ForeignKey(nameof(DocumentId))]
    public F003Document Document { get; set; }
    
    [Column("date_edit")]
    public DateTime DateEdit { get; set; }
}