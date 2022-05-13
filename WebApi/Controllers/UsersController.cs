namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private readonly IUltilitiesService _ultilitiesService;

    public UsersController(IUserService userService, IUltilitiesService ultilitiesService)
    {
        _userService = userService;
        _ultilitiesService = ultilitiesService;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model);
        return Ok(response);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var users = await _userService.GetAll();
        var usersSameLocation = users.Where(u => u.Location.Equals(currentAdmin?.Location));
        return Ok(usersSameLocation);
    }

    [HttpPut("firsttimepassword")]
    public async Task<IActionResult> ChangePasswordFirstTime(ChangePasswordRequest model)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        if (model.Id != currentUser.Id) return Unauthorized(new { message = "Permission are not allowed" });
        if (currentUser.IsFirstLogin == false) return BadRequest(new { message = "You already changed your password" });
        await _userService.UpdatePasswordFirstTime(model);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        var user = await _userService.GetById(id);
        if (id != currentUser?.Id && currentUser?.Type != Role.Admin) return Unauthorized(new { message = "Unauthorized" });
        if (user.Location.Equals(currentUser.Location)) return Unauthorized("Can not view detail of Staff with different location");
        return Ok(user);
    }

    [Authorize(Role.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateNewUser(UserCreateModel user)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        if (currentAdmin?.Type != Role.Admin) return Unauthorized(new { message = "Unauthorized" });
        var newUser = await _userService.CreateUser(user, currentAdmin.Location.ToString(), currentAdmin.Id);
        return Ok(newUser);
    }

    [Authorize(Role.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserUpdateModel user)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        if (currentAdmin?.Type != Role.Admin) return Unauthorized(new { message = "Unauthorized" });
        await _userService.UpdateUser(user, currentAdmin.Location.ToString());
        return Ok();
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");

        await _userService.Logout(mwi.Exp, mwi.Token);
        await _userService.DeleteExpirationDateToken();
        return Ok();
    }


    [Authorize(Role.Admin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        if (currentAdmin.Id == id) return BadRequest(new { message = "You can not disable yourself" });
        await _userService.DeleteUser(id);
        return Ok();
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword(PasswordRequest model)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        await _userService.UpdatePassword(model.OldPassword, model.NewPassword, currentUser.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetAllPagination(string? filter = null, string? search = null, string? sort = null, string? sortTerm = "staffcode", int page = 1, int pageSize = 10)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var users = await _userService.GetAll();
        var usersSameLocation = users.Where(u => u.Location.Equals(currentAdmin.Location));
        var pagination = _ultilitiesService.GetUserResults(usersSameLocation, filter, search, sort, sortTerm, page, pageSize);
        return Ok(pagination);
    }
}