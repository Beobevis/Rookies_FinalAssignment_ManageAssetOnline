
namespace WebApi.Models.Users
{
    public class UserUpdateModel
    {
    public int Id {get; set;}
    //public string? Firstname { get; set; }
    //public string? Lastname { get; set; }
    public DateTime JoinDate {get; set;}
    public string? Type { get; set; }
    public DateTime DoB {get; set;}
    public string? Gender {get; set;}

    }
}