using Domain.Models.Common;

namespace Domain.Models.Entities;

public class UserRefreshToken:BaseEntity
{
    public string? UserName { get; set; }

    public string? RefreshToken { get; set; }
    
    public DateTime? ExpiredTime { get; set; }
}