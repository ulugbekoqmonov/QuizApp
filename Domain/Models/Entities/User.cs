﻿using Domain.Models.Common;

namespace Domain.Models.Entities;

public class User : BaseAuditableEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid? RoleId { get; set; }
    public virtual Role? Role { get; set; }
}