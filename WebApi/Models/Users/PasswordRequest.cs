namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class PasswordRequest
{
    [Required]
    public string OldPassword { get; set; } = null!;
    [Required]
    public string NewPassword { get; set; } = null!;
}