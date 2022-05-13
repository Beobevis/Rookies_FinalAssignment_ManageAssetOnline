namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;
using ClosedXML.Excel;
using System.IO;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private IUserService _userService;
    private IReportService _reportService;
    private readonly IUltilitiesService _ultilitiesService;

    public ReportController(IUserService userService, IReportService reportService, IUltilitiesService ultilitiesService)
    {
        _userService = userService;
        _reportService = reportService;
        _ultilitiesService = ultilitiesService;
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var report = await _reportService.GetReport(currentAdmin.Location);
        return Ok(report);
    }

    [Authorize(Role.Admin)]
    [HttpGet("exportfile")]
    public async Task<IActionResult> ExportFile()
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;

        var assets = await _reportService.GetReport(currentAdmin.Location);
        var report = assets.ToList();
        //required using ClosedXML.Excel;
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        string fileName = "assetsreport.xlsx";
        try
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Assets Report");
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Category";
                worksheet.Cell(1, 3).Value = "Total";
                worksheet.Cell(1, 4).Value = "Assigned";
                worksheet.Cell(1, 5).Value = "Available";
                worksheet.Cell(1, 6).Value = "Not available";
                worksheet.Cell(1, 7).Value = "Waiting for recycling";
                worksheet.Cell(1, 8).Value = "Recycled";

                for (int index = 1; index <= report.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = report[index - 1].CategoryId;
                    worksheet.Cell(index + 1, 2).Value = report[index - 1].CategoryName;
                    worksheet.Cell(index + 1, 3).Value = report[index - 1].Total;
                    worksheet.Cell(index + 1, 4).Value = report[index - 1].Assigned;
                    worksheet.Cell(index + 1, 5).Value = report[index - 1].Available;
                    worksheet.Cell(index + 1, 6).Value = report[index - 1].Notavailable;
                    worksheet.Cell(index + 1, 7).Value = report[index - 1].Waitingforrecycling;
                    worksheet.Cell(index + 1, 8).Value = report[index - 1].Recycled;
                }
                //required using System.IO;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Role.Admin)]
    [HttpGet("pagination")]
    public async Task<IActionResult> GetAllPagination(string? sort = "asc", string? sortTerm = "category", int page = 1, int pageSize = 10)
    {
        MiddlewareInfo? mwi = HttpContext.Items["UserTokenInfo"] as MiddlewareInfo;
        if (mwi == null) return Unauthorized("You must login to see this information");
        var currentAdmin = (User)mwi.User;
        
        var report = await _reportService.GetReport(currentAdmin.Location);
        var pagination = _ultilitiesService.GetReportResults(report, sort, sortTerm, page, pageSize);
        return Ok(pagination);
    }
}