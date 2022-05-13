namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReturnRequestsController : ControllerBase
{
    private IReturnService _returnService;
    private IUserService _userService;
    private readonly IUltilitiesService _ultilitiesService;

    public ReturnRequestsController(IReturnService returnService, IUserService userService, IUltilitiesService ultilitiesService)
    {
        _returnService = returnService;
        _userService = userService;
        _ultilitiesService = ultilitiesService;
    }

    [HttpPost("user/{assignmentid:int}")]
    public async Task<IActionResult> CreateReturnRequestUser(int assignmentid)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        await _returnService.CreateReturnRequestUser(assignmentid, currentUser.Location.ToString(), currentUser.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpPost("admin/{assignmentid:int}")]
    public async Task<IActionResult> CreateReturnRequestAdmin(int assignmentid)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _returnService.CreateReturnRequestAdmin(assignmentid, currentAdmin.Location.ToString(), currentAdmin.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReturnRequest(int id)
    {
       MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var request = await _returnService.GetReturnRequest(id);
        if (request == null) return NotFound();
        if (!request.Location.Equals(currentAdmin.Location)) return Unauthorized("Permission to see this request denied");
        return Ok(request);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAllReturnRequests()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var requests = await _returnService.GetAllReturnRequests();
        var requestsSameLocation = requests.Where(r => r.Location.Equals(currentAdmin.Location));
        return Ok(requestsSameLocation);
    }

    [Authorize(Role.Admin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteReturnRequest(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _returnService.DeleteReturnRequest(id, currentAdmin.Location.ToString());
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpPut]
    public async Task<IActionResult> AcceptReturnRequest(int requestid)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _returnService.AcceptReturnRequest(requestid, currentAdmin.Id, currentAdmin.Location.ToString());
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetAllReturnRequestsPagination(string? filterstate = null, DateTime filterreturndate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var requests = await _returnService.GetAllReturnRequests();
        var requestsSameLocation = requests.Where(r => r.Location.Equals(currentAdmin.Location));
        var pagination = _ultilitiesService.GetReturnResults(requestsSameLocation, filterstate, filterreturndate, search, sort, sortTerm, page, pageSize);
        return Ok(pagination);
    }
}
