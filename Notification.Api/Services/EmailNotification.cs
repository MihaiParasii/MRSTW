using Notification.Api.Models;

namespace Notification.Api.Services;

public class EmailNotification(IEmailService emailService) : INotification
{
    public async Task NotifyAsync(SendNotificationRequest request)
    {
        var emailRequest = new SendEmailRequest
        {
            DealAuthorName = request.DealAuthorName,
            DealAuthorPhoneNumber = request.DealAuthorPhoneNumber,
            RecipientName = request.RecipientName,
            RecipientEmail = request.RecipientEmail,
            Subject = request.Subject,
            Message = request.Message
        };
        
        await emailService.SendEmailAsync(emailRequest);
    }
}
