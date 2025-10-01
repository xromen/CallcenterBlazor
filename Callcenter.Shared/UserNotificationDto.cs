namespace Callcenter.Shared;

public class UserNotificationDto
{
    public int Id { get; set; }
    
    public int DeclarationId { get; set; }
    
    public UserDto UserWhoSend { get; set; }
    
    public DateTime Date { get; set; }
    
    public string NotificationTypeName { get; set; }
}