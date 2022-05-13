using WebApi.Entities;
using WebApi.Models.Assets;
using WebApi.Models.Assignments;
using WebApi.Models.Pagination;
using WebApi.Models.Report;
using WebApi.Models.ReturnRequest;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IUltilitiesService{
    UserPagination GetUserResults(IEnumerable<User> list, string? filter = null, string? search = null, string? sort = null, string? sortTerm = "staffcode", int page = 1, int pageSize = 10);
    AssetPagination GetAssetResults(IEnumerable<AssetModel> list, string? filterstate = null, string? filtercategory = null, string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    AssignmentPagination GetAssignmentResults(IEnumerable<AssignmentModel> list, string? filterstate = null, DateTime filterassigneddate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    ReturnRequestPagination GetReturnResults(IEnumerable<ReturnRequestModel> list, string? filterstate = null, DateTime filterreturndate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    ReportPagination GetReportResults(IEnumerable<ReportModel> list, string? sort = "asc", string? sortTerm = "category", int page = 1, int pageSize = 10);
}

public class UltilitiesService : IUltilitiesService
{
    private readonly IUltilitiesRepository _ultilitiesRepository;
    public UltilitiesService(IUltilitiesRepository ultilitiesRepository)
    {
        _ultilitiesRepository = ultilitiesRepository;
    }
    
    public UserPagination GetUserResults(IEnumerable<User> list, string? filter = null, string? search = null, string? sort = null, string? sortTerm = "staffcode", int page = 1, int pageSize = 10){
        return _ultilitiesRepository.GetUserResults(list, filter, search, sort, sortTerm, page, pageSize);
    }
    public AssetPagination GetAssetResults(IEnumerable<AssetModel> list, string? filterstate = null, string? filtercategory = null, string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10){
        return _ultilitiesRepository.GetAssetResults(list, filterstate, filtercategory, search, sort, sortTerm, page, pageSize);
    }
    public AssignmentPagination GetAssignmentResults(IEnumerable<AssignmentModel> list, string? filterstate = null, DateTime filterassigneddate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10){
        return _ultilitiesRepository.GetAssignmentResults(list, filterstate, filterassigneddate, search, sort, sortTerm, page, pageSize);
    }
    public ReturnRequestPagination GetReturnResults(IEnumerable<ReturnRequestModel> list, string? filterstate = null, DateTime filterreturndate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10){
        return _ultilitiesRepository.GetReturnResults(list, filterstate, filterreturndate, search, sort, sortTerm, page, pageSize);
    }
    public ReportPagination GetReportResults(IEnumerable<ReportModel> list, string? sort = "asc", string? sortTerm = "category", int page = 1, int pageSize = 10){
        return _ultilitiesRepository.GetReportResults(list, sort, sortTerm, page, pageSize);
    }
}