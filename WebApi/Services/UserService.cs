using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task UpdatePasswordFirstTime(ChangePasswordRequest user);
    Task<CreateUserResponse> CreateUser(UserCreateModel user, string Location, int adminid);
    Task UpdateUser(UserUpdateModel user, string adminlocation);
    Task<bool> CheckTokenLoggedout(string token);
    Task Logout(DateTime exp, string token);
    Task DeleteUser(int id);
    Task DeleteExpirationDateToken();
    Task UpdatePassword(string oldpassword, string newpassword, int userid);

}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        return await _userRepository.Authenticate(model);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> GetById(int id)
    {
        return await _userRepository.GetById(id);
    }

    public async Task UpdatePasswordFirstTime(ChangePasswordRequest user)
    {
        await _userRepository.UpdatePasswordFirstTime(user);
    }

    public async Task<CreateUserResponse> CreateUser(UserCreateModel user, string Location, int adminid)
    {
        return await _userRepository.CreateUser(user, Location, adminid);
    }

    public async Task UpdateUser(UserUpdateModel user, string adminlocation)
    {
        await _userRepository.UpdateUser(user, adminlocation);
    }

    public async Task<bool> CheckTokenLoggedout(string token)
    {
        return await _userRepository.CheckTokenLoggedout(token);
    }
    public async Task Logout(DateTime exp, string token)
    {
        await _userRepository.Logout(exp, token);
    }

    public async Task DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);
    }

    public async Task DeleteExpirationDateToken()
    {
        await _userRepository.DeleteExpirationDateToken();
    }

    public async Task UpdatePassword(string oldpassword, string newpassword, int userid)
    {
        await _userRepository.UpdatePassword(oldpassword, newpassword, userid);
    }

}