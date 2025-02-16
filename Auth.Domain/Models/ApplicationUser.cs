using Auth.Domain.Common;

namespace Auth.Domain.Models;

public class ApplicationUser : BaseUser
{
    public string PhoneNumber { get; set; } = string.Empty;

    // public List<Deal> Deals { get; set; } = []; 
}
