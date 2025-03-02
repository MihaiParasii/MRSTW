namespace Notification.Api.Models;

public class SendNotificationRequest
{
    public required string DealAuthorName { get; set; }
    public required string DealAuthorPhoneNumber { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientEmail { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
}