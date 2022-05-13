namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Assets;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private IAssetService _assetService;
    private IUserService _userService;
    private readonly IUltilitiesService _ultilitiesService;

    public AssetsController(IAssetService assetService, IUserService userService, IUltilitiesService ultilitiesService)
    {
        _assetService = assetService;
        _userService = userService;
        _ultilitiesService = ultilitiesService;
    }

    [Authorize(Role.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateAsset(AssetCreateModel asset)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assetService.CreateAsset(asset, currentAdmin.Location.ToString(), currentAdmin.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAllAssets()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var assets = await _assetService.GetAllAssets();
        var assetsSameLocation = assets.Where(a => a.Location.Equals(currentAdmin.Location));
        return Ok(assetsSameLocation);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsset(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;
        
        var asset = await _assetService.GetAsset(id);
        if (!asset.Location.Equals(((User)mwi.User).Location)) return Unauthorized(new { message = "Permission are not allowed" });
        return Ok(asset);
    }

    [Authorize(Role.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateAsset(AssetUpdateModel asset)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assetService.UpdateAsset(asset, currentAdmin.Location.ToString());
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assetService.DeleteAsset(id, currentAdmin.Location.ToString());
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetAllAssetsPagination(string? filterstate = null, string? filtercategory = null, string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var assets = await _assetService.GetAllAssets();
        var assetsSameLocation = assets.Where(a => a.Location.Equals(currentAdmin?.Location));
        var pagination = _ultilitiesService.GetAssetResults(assetsSameLocation, filterstate, filtercategory, search, sort, sortTerm, page, pageSize);
        return Ok(pagination);
    }
}