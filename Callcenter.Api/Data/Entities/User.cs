using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("group_id")]
    public int GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public UserGroup Group { get; set; }
    
    [Column("login")]
    public string Login { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("org_id")]
    public int OrgId { get; set; }
    
    [ForeignKey(nameof(OrgId))]
    public Organisation Organisation { get; set; }
    
    [Column("is_enabled")]
    public bool IsEnabled { get; set; }
    
    [Column("parent_user_id")]
    public int ParentUserId { get; set; }
    
    [Column("date_added")]
    public DateTime DateAdded { get; set; }
    
    [Column("full_name")]
    public string FullName { get; set; }
    
    [Column("is_ruk")]
    public bool IsRuk { get; set; }
    
    [Column("sp_level")]
    public int? SpLevel { get; set; }
}