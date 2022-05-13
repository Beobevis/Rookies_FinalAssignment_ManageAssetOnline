using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
namespace WebApi.Models.Users
{
    public class UserCreateModel
    {
    [Required]
    public string Firstname { get; set; } = null!;
    [Required]
    public string Lastname { get; set; } = null!;
    public DateTime JoinDate {get; set;}
    public string? Type { get; set; }
    public DateTime DoB {get; set;}
    public string? Gender {get; set;}
    }
}