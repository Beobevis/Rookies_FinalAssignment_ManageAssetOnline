namespace WebApi.Models.Users;

using WebApi.Entities;

public class ChangePasswordResponse
{
    public string Username {get; set; }
    public string Message { get; set; }

    public ChangePasswordResponse(string user,string mess)
    {
        Username = user;
        Message = mess;
    }
}