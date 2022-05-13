using WebApi.Models.Report;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IReportService
{
    Task<IEnumerable<ReportModel>> GetReport(string adminlocation);
}

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<IEnumerable<ReportModel>> GetReport(string adminlocation)
    {
        return await _reportRepository.GetReport(adminlocation);
    }
}