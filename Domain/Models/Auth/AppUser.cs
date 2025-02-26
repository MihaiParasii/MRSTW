using Domain.Common;

namespace Domain.Models.Auth;

public class AppUser : BaseUser
{
    public string PhoneNumber { get; set; } = string.Empty;
}
