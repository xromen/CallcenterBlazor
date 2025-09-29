using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Models.Reports;

public class OmsPerformanceCriteriaFormModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("full_name")]
    public string FullName { get; set; }
    
    [Column("coc")]
    public long Coc { get; set; } //Что за сос?
    
    [Column("ust")]
    public long Ust { get; set; }
    
    [Column("pis")]
    public long Pis { get; set; }
    
    [Column("osh")]
    public long Osh { get; set; }
}