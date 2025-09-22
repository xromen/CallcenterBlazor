using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_org_numbers")]
public class OrganisationName
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("org_id")]
    public int OrganisationId { get; set; }
    
    [ForeignKey(nameof(OrganisationId))]
    public Organisation Organisation { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}