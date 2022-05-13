namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Assets;
using WebApi.Models.Categories;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private IUserService _userService;

    private IGenericService<CategoryModel> _categoryService;

    public CategoryController(IUserService userService, IGenericService<CategoryModel> categoryService)
    {
        _userService = userService;
        _categoryService = categoryService;
    }
    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        return Ok(await _categoryService.GetAllAsync());
    }

    [Authorize(Role.Admin)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        return Ok(await _categoryService.GetByIdAsync(id));
    }

    [Authorize(Role.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryModel model)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        if (ModelState.IsValid)
        {
            await _categoryService.CreateAsync(model, currentAdmin.Id);
            return Ok("success");
        }
        return BadRequest(model);
    }

    [Authorize(Role.Admin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(int id){
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;
        
        await _categoryService.DeleteAsync(id);
        return Ok("success");
    }
}