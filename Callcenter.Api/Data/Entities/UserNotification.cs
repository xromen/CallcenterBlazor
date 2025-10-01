using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_user_warning")]
public class UserNotification
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("user_id")]
    public int? UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [Column("rk_id")]
    public int DeclarationId { get; set; }
    
    [ForeignKey(nameof(DeclarationId))]
    public Declaration Declaration { get; set; }
    
    [Column("did_see")]
    public bool IsRead { get; set; }
    
    [Column("who_send")]
    public int WhoSend { get; set; }
    
    [ForeignKey(nameof(WhoSend))]
    public User UserWhoSend { get; set; }
    
    [Column("action_id")]
    public int NotificationTypeId { get; set; }
    
    [ForeignKey(nameof(NotificationTypeId))]
    public NotificationType NotificationType { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
}