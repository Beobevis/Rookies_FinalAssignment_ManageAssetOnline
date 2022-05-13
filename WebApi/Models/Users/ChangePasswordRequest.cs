namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class ChangePasswordRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string NewPassword {get; set;} = null!;
}