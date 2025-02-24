namespace MRSTW.Application.Common.Models;

public class SendEmailRequest
{
    public string FromEmail { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
}
