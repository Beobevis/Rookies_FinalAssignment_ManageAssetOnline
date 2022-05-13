namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Assignments;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssignmentsController : ControllerBase
{
    private IAssignmentService _assignmentService;
    private IUserService _userService;
    private readonly IUltilitiesService _ultilitiesService;

    public AssignmentsController(IAssignmentService assignmentService, IUserService userService, IUltilitiesService ultilitiesService)
    {
        _assignmentService = assignmentService;
        _userService = userService;
        _ultilitiesService = ultilitiesService;
    }

    [Authorize(Role.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateAssignment(AssignmentCreateModel assignment)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assignmentService.CreateAssignment(assignment, currentAdmin.Location.ToString(), currentAdmin.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpPut]
    public async Task<IActionResult> UpdateAssignment(AssignmentUpdateModel assignment)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assignmentService.UpdateAssignment(assignment, currentAdmin.Location.ToString(), currentAdmin.Id);
        return Ok();
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAllAssignments()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var assignments = await _assignmentService.GetAllAssignments();
        var assignmentsSameLocation = assignments.Where(a => a.Location.Equals(currentAdmin?.Location));
        return Ok(assignmentsSameLocation);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAssignment(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        var assignment = await _assignmentService.GetAssignment(id);
        if (!assignment.Location.Equals(currentUser?.Location)) return Unauthorized(new { message = "Unauthorized" });
        if (currentUser?.Type == Role.Admin || assignment.AssignToId == currentUser?.Id)
            return Ok(assignment);
        return Unauthorized(new { message = "Permission to see this assignment is denied" });
    }

    [Authorize(Role.Admin)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        await _assignmentService.DeleteAssignment(id, currentAdmin.Location);
        return Ok();
    }

    [HttpPut("{id:int}/{state}")]
    public async Task<IActionResult> UpdateAssignmentStatus(int id, string state) // assignment id and assignment state
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;

        await _assignmentService.UpdateAssignmnetState(id, state, currentUser.Id);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetAssignmentsByUser()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentUser = (User)mwi.User;
        
        var assignments = await _assignmentService.GetAssignmentsByUser(currentUser.Id);
        return Ok(assignments);
    }

    [Authorize(Role.Admin)]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetAllAssignmentsPagination(string? filterstate = null, DateTime filterassigneddate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var assignments = await _assignmentService.GetAllAssignments();
        var assignmentsSameLocation = assignments.Where(a => a.Location.Equals(currentAdmin?.Location));
        var pagination = _ultilitiesService.GetAssignmentResults(assignmentsSameLocation, filterstate, filterassigneddate, search, sort, sortTerm, page, pageSize);
        return Ok(pagination);
    }
}