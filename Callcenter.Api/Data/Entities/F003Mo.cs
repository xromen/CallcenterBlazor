using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("f003_mo", Schema = "registers")]
public class F003Mo
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("mcod")]
    public Guid McodId { get; set; }
    
    [ForeignKey(nameof(McodId))]
    public F003MoMcod Mcod { get; set; }
    
    [Column("nam_mok")]
    public string Name { get; set; }

    [InverseProperty(nameof(F003MoDocument.Mo))]
    public List<F003MoDocument> MoDocuments { get; set; } = new();
}