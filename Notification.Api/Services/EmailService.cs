using Notification.Api.Models;

namespace Notification.Api.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(Email email)
    {
        string? sendgridApiKey = configuration["SendGridSettings:ApiKey"];
        string? senderEmail = configuration["SendGridSettings:FromEmail"];
        string? senderName = configuration["SendGridSettings:FromName"];

         throw new NotImplementedException();
         /*
         var client = new SendGridClient(sendgridApiKey);
         var from = new EmailAddress(senderEmail, senderName);
         var to = new EmailAddress(toEmail);
         var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
         var response = await client.SendEmailAsync(msg);

         if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
         {
             // Handle error
             var responseBody = await response.Body.ReadAsStringAsync();
             throw new Exception($"Failed to send email: {response.StatusCode}, {responseBody}");
         }
         */
    }
}
