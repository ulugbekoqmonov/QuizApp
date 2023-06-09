using Domain.Models.Common;

namespace Domain.Models.Entities;

public class User : BaseAuditableEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>()
    {
        new()
        {
        RoleName="User",
        }
    };
}