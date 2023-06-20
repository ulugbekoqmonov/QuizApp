using Application.Extersion;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.User;

public class UpdateUserDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "FirstName is required.")]
    [Range(3, 50)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required.")]
    [Range(3, 50)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "UserName is required.")]
    [Range(3, 50)]
    public string? UserName { get; set; }

    private string _Password;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must contain at least 8 characters, including both letters and numbers.")]
    public string Password
    {
        get
        {
            return _Password;
        }
        set
        {
            _Password = value.ComputeHash();
        }
    }

    private string _ConfirmPassword;
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword
    {
        get
        {
            return _ConfirmPassword;
        }
        set
        {
            _ConfirmPassword = value.ComputeHash();
        }
    }

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
    
    [RegularExpression(@"^\+998\d{9}$", ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }
}
