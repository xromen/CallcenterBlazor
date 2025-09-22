using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("f003_mcod", Schema = "registers")]
public class F003MoMcod
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("mcod")]
    public int Mcod { get; set; }
    
    [Column("oms_priz")]
    public bool OmsPriz { get; set; }
}