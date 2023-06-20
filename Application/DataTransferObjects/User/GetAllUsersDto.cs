﻿namespace Application.DataTransferObjects.User;

public class GetAllUsersDto
{
    public Guid Id {  get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? RoleId { get; set; }
}
