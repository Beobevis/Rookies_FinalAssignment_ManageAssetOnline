namespace WebApi.Repositories;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Report;

public interface IReportRepository
{
    Task<IEnumerable<ReportModel>> GetReport(string adminlocation);
}

public class ReportRepository : IReportRepository
{
    private readonly DataContext _context;
    public ReportRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReportModel>> GetReport(string adminlocation)
    {
        IEnumerable<ReportModel> reportlist = new List<ReportModel>();
        var categories = await _context.Categories.ToListAsync();
        var assets = await _context.Assets.ToListAsync();
        foreach (var cat in categories)
        {
            var report = new ReportModel
            {
                CategoryId = cat.Id,
                CategoryName = cat.CategoryName,
                Total = assets.Where(a => a.CategoryId == cat.Id && a.Location.Equals(adminlocation)).Count(),
                Assigned = assets.Where(a => a.CategoryId == cat.Id && a.State == AssetState.Assigned && a.Location.Equals(adminlocation)).Count(),
                Available = assets.Where(a => a.CategoryId == cat.Id && a.State == AssetState.Available && a.Location.Equals(adminlocation)).Count(),
                Notavailable = assets.Where(a => a.CategoryId == cat.Id && a.State == AssetState.NotAvailable && a.Location.Equals(adminlocation)).Count(),
                Waitingforrecycling = assets.Where(a => a.CategoryId == cat.Id && a.State == AssetState.WaitingForRecycling && a.Location.Equals(adminlocation)).Count(),
                Recycled = assets.Where(a => a.CategoryId == cat.Id && a.State == AssetState.Recycled && a.Location.Equals(adminlocation)).Count()
            };

            reportlist = reportlist.Append(report);
        }
        return reportlist;
    }
}

