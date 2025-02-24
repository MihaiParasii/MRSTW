namespace Auth.Domain.Models;

public class ErrorModel
{
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
