using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_svo_statuses")]
public class SvoStatus
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("status")]
    public string Name { get; set; }
}