using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_mo_region")]
public class MoRegion
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("mcod")]
    public int Code { get; set; }
    
    [Column("region_id")]
    public int RegionId { get; set; }
    
    [Column("name_mo")]
    public string Name { get; set; }
}