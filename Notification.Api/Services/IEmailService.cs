using Notification.Api.Models;

namespace Notification.Api.Services;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest request);
}
