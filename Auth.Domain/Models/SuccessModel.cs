namespace Auth.Domain.Models;

public class SuccessModel
{
    public bool IsSuccess { get; set; }
    public object Data { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
}
