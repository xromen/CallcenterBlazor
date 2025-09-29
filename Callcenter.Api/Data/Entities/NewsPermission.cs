using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_permissions_news")]
public class NewsPermission
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("news_id")]
    public int NewsId { get; set; }
    
    [ForeignKey(nameof(NewsId))]
    public News News { get; set; }
    
    [Column("org_id")]
    public int OrganizationId { get; set; }
    
    [ForeignKey(nameof(OrganizationId))]
    public Organisation Organization { get; set; }
}