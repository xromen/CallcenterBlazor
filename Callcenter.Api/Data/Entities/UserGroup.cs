using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_user_group")]
public class UserGroup
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("org_id")]
    public int OrgId { get; set; }
}