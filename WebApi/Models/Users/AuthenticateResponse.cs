namespace WebApi.Models.Users;

using WebApi.Entities;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public Role Role { get; set; }
    public bool IsFirstLogin { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Username = user.Username;
        Role = user.Type;
        IsFirstLogin = user.IsFirstLogin;
        Token = token;
    }
}