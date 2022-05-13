namespace WebApi.Models.Users;
using WebApi.Entities;

public class MiddlewareInfo{
    public User User {get; set;} = null!;
    public DateTime Exp {get; set;}
    public string Token {get; set;} = null!;
}