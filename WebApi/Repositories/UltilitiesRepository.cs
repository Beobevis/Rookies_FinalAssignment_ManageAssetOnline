using WebApi.Entities;
using WebApi.Models.Assets;
using WebApi.Helpers;
using WebApi.Models.Assignments;
using WebApi.Models.ReturnRequest;
using WebApi.Models.Report;
using WebApi.Models.Pagination;

namespace WebApi.Repositories;

public interface IUltilitiesRepository
{
    UserPagination GetUserResults(IEnumerable<User> list, string? filter = null, string? search = null, string? sort = null, string? sortTerm = "staffcode", int page = 1, int pageSize = 10);
    AssetPagination GetAssetResults(IEnumerable<AssetModel> list, string? filterstate = null, string? filtercategory = null, string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    AssignmentPagination GetAssignmentResults(IEnumerable<AssignmentModel> list, string? filterstate = null, DateTime filterassigneddate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    ReturnRequestPagination GetReturnResults(IEnumerable<ReturnRequestModel> list, string? filterstate = null, DateTime filterreturndate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10);
    ReportPagination GetReportResults(IEnumerable<ReportModel> list, string? sort = "asc", string? sortTerm = "category", int page = 1, int pageSize = 10);
}

public class UltilitiesRepository : IUltilitiesRepository
{
    private readonly DataContext _context;

    public UltilitiesRepository(DataContext context)
    {
        _context = context;
    }
    // User list pagination, search, filter, sort
    public UserPagination GetUserResults(IEnumerable<User> list, string? filter = null, string? search = null, string? sort = "asc", string? sortTerm = "staffcode", int page = 1, int pageSize = 10)
    {
        IEnumerable<User> filterlist = list;
        if (filter != null)
        {
            var filterType = filter.Trim().ToLower();
            if (filterType.Equals("admin")) filterlist = list.Where(l => l.Type.Equals(Role.Admin));
            if (filterType.Equals("staff")) filterlist = list.Where(l => l.Type.Equals(Role.Staff));
        }

        IEnumerable<User> searchlist = filterlist;
        if (search != null)
        {
            var searchString = search.ToLower().Trim();
            searchlist = filterlist.Where(x => x.StaffCode.ToLower().Contains(search) || x.Firstname.ToLower().Contains(search) || x.Lastname.ToLower().Contains(search));
        }

        IEnumerable<User> sortlist = searchlist;
        if (sort != null && sortTerm != null)
        {
            var sortOrder = sort.Trim().ToLower();
            if (sortOrder.Equals("asc"))
            {
                if (sortTerm.Trim().ToLower().Equals("staffcode")) sortlist = searchlist.OrderBy(x => x.StaffCode);
                if (sortTerm.Trim().ToLower().Equals("fullname")) sortlist = searchlist.OrderBy(x => x.Firstname).ThenBy(x => x.Lastname);
                if (sortTerm.Trim().ToLower().Equals("joindate")) sortlist = searchlist.OrderBy(x => x.JoinDate);
            }
            if (sortOrder.Equals("desc"))
            {
                if (sortTerm.Trim().ToLower().Equals("staffcode")) sortlist = searchlist.OrderByDescending(x => x.StaffCode);
                if (sortTerm.Trim().ToLower().Equals("fullname")) sortlist = searchlist.OrderByDescending(x => x.Firstname).ThenByDescending(x => x.Lastname);
                if (sortTerm.Trim().ToLower().Equals("joindate")) sortlist = searchlist.OrderByDescending(x => x.JoinDate);
            }
        }

        IEnumerable<User> pagelist = sortlist;
        if (pageSize > 0)
        {
            pagelist = sortlist.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return new UserPagination
        {
            Users = pagelist,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = CalculateCeiling(sortlist.Count(), pageSize),
        };
    }


    // Asset list pagination, search, filter, sort
    public AssetPagination GetAssetResults(IEnumerable<AssetModel> list, string? filterstate = null, string? filtercategory = null, string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        IEnumerable<AssetModel> filterlist = list;
        if (filterstate != null)
        {
            var filterType = filterstate.Trim().ToLower();
            if (filterType.Equals("assigned")) filterlist = list.Where(l => l.State.Equals(AssetState.Assigned));
            if (filterType.Equals("available")) filterlist = list.Where(l => l.State.Equals(AssetState.Available));
            if (filterType.Equals("notavailable")) filterlist = list.Where(l => l.State.Equals(AssetState.NotAvailable));
            if (filterType.Equals("watingforrecycling")) filterlist = list.Where(l => l.State.Equals(AssetState.WaitingForRecycling));
            if (filterType.Equals("recycled")) filterlist = list.Where(l => l.State.Equals(AssetState.Recycled));
        }

        if (filtercategory != null)
        {
            filterlist = filterlist.Where(l => l.CategoryName.Trim().ToLower().Equals(filtercategory.Trim().ToLower()));
        }

        IEnumerable<AssetModel> searchlist = filterlist;
        if (search != null)
        {
            var searchString = search.ToLower().Trim();
            searchlist = filterlist.Where(x => x.AssetCode.ToLower().Contains(search) || x.AssetName.ToLower().Contains(search));
        }

        IEnumerable<AssetModel> sortlist = searchlist;
        if (sort != null && sortTerm != null)
        {
            var sortOrder = sort.Trim().ToLower();
            if (sortOrder.Equals("asc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderBy(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderBy(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("assetstate")) sortlist = searchlist.OrderBy(x => x.State);
                if (sortTerm.Trim().ToLower().Equals("category")) sortlist = searchlist.OrderBy(x => x.CategoryName);
            }
            if (sortOrder.Equals("desc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderByDescending(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderByDescending(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("assetstate")) sortlist = searchlist.OrderByDescending(x => x.State);
                if (sortTerm.Trim().ToLower().Equals("category")) sortlist = searchlist.OrderByDescending(x => x.CategoryName);
            }
        }

        IEnumerable<AssetModel> pagelist = sortlist;
        if (pageSize > 0)
        {
            pagelist = sortlist.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return new AssetPagination
        {
            Assets = pagelist,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = CalculateCeiling(sortlist.Count(), pageSize),
        };
    }

    // Assignment list pagination, search, filter, sort
    public AssignmentPagination GetAssignmentResults(IEnumerable<AssignmentModel> list, string? filterstate = null, DateTime filterassigneddate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        IEnumerable<AssignmentModel> filterlist = list;
        if (filterstate != null)
        {
            var filterType = filterstate.Trim().ToLower();
            if (filterType.Equals("accepted")) filterlist = list.Where(l => l.State.Equals(AssignmentState.Accepted));
            if (filterType.Equals("declined")) filterlist = list.Where(l => l.State.Equals(AssignmentState.Declined));
            if (filterType.Equals("waitingforacceptance")) filterlist = list.Where(l => l.State.Equals(AssignmentState.WaitingForAcceptance));
        }

        if (filterassigneddate != new DateTime())
        {
            filterlist = filterlist.Where(l => DateTime.Compare(l.AssignedDate.Date, filterassigneddate.Date) == 0);
        }

        IEnumerable<AssignmentModel> searchlist = filterlist;
        if (search != null)
        {
            var searchString = search.ToLower().Trim();
            searchlist = filterlist.Where(x => x.AssetCode.ToLower().Contains(search) || x.AssetName.ToLower().Contains(search) || x.AssignTo.ToLower().Contains(search));
        }

        IEnumerable<AssignmentModel> sortlist = searchlist;
        if (sort != null && sortTerm != null)
        {
            var sortOrder = sort.Trim().ToLower();
            if (sortOrder.Equals("asc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderBy(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderBy(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("assignto")) sortlist = searchlist.OrderBy(x => x.AssignTo);
                if (sortTerm.Trim().ToLower().Equals("assignby")) sortlist = searchlist.OrderBy(x => x.AssignedBy);
                if (sortTerm.Trim().ToLower().Equals("assigneddate")) sortlist = searchlist.OrderBy(x => x.AssignedDate);
                if (sortTerm.Trim().ToLower().Equals("assignmentstate")) sortlist = searchlist.OrderBy(x => x.State);
            }
            if (sortOrder.Equals("desc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderByDescending(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderByDescending(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("assignto")) sortlist = searchlist.OrderByDescending(x => x.AssignTo);
                if (sortTerm.Trim().ToLower().Equals("assignby")) sortlist = searchlist.OrderByDescending(x => x.AssignedBy);
                if (sortTerm.Trim().ToLower().Equals("assigneddate")) sortlist = searchlist.OrderByDescending(x => x.AssignedDate);
                if (sortTerm.Trim().ToLower().Equals("assignmentstate")) sortlist = searchlist.OrderByDescending(x => x.State);
            }
        }

        IEnumerable<AssignmentModel> pagelist = sortlist;
        if (pageSize > 0)
        {
            pagelist = sortlist.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return new AssignmentPagination
        {
            Assignments = pagelist,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = CalculateCeiling(sortlist.Count(), pageSize),
        };
    }

    public ReturnRequestPagination GetReturnResults(IEnumerable<ReturnRequestModel> list, string? filterstate = null, DateTime filterreturndate = new DateTime(), string? search = null, string? sort = "asc", string? sortTerm = "assetcode", int page = 1, int pageSize = 10)
    {
        IEnumerable<ReturnRequestModel> filterlist = list;
        if (filterstate != null)
        {
            var filterType = filterstate.Trim().ToLower();
            if (filterType.Equals("completed")) filterlist = list.Where(l => l.State.Equals(ReturnState.Completed));
            if (filterType.Equals("waitingforreturn")) filterlist = list.Where(l => l.State.Equals(ReturnState.WaitingForReturn));
        }

        if (filterreturndate != new DateTime())
        {
            filterlist = filterlist.Where(l => DateTime.Compare(l.ReturnedDate.Date, filterreturndate.Date) == 0);
        }

        IEnumerable<ReturnRequestModel> searchlist = filterlist;
        if (search != null)
        {
            var searchString = search.ToLower().Trim();
            searchlist = filterlist.Where(x => x.AssetCode.ToLower().Contains(search) || x.AssetName.ToLower().Contains(search) || x.RequestedBy.ToLower().Contains(search));
        }

        IEnumerable<ReturnRequestModel> sortlist = searchlist;
        if (sort != null && sortTerm != null)
        {
            var sortOrder = sort.Trim().ToLower();
            if (sortOrder.Equals("asc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderBy(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderBy(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("requestedby")) sortlist = searchlist.OrderBy(x => x.RequestedBy);
                if (sortTerm.Trim().ToLower().Equals("assigneddate")) sortlist = searchlist.OrderBy(x => x.AssignedDate);
                if (sortTerm.Trim().ToLower().Equals("acceptedby")) sortlist = searchlist.OrderBy(x => x.AcceptedBy);
                if (sortTerm.Trim().ToLower().Equals("returneddate")) sortlist = searchlist.OrderBy(x => x.ReturnedDate);
                if (sortTerm.Trim().ToLower().Equals("returnstate")) sortlist = searchlist.OrderBy(x => x.State);
            }
            if (sortOrder.Equals("desc"))
            {
                if (sortTerm.Trim().ToLower().Equals("assetcode")) sortlist = searchlist.OrderByDescending(x => x.AssetCode);
                if (sortTerm.Trim().ToLower().Equals("assetname")) sortlist = searchlist.OrderByDescending(x => x.AssetName);
                if (sortTerm.Trim().ToLower().Equals("requestedby")) sortlist = searchlist.OrderByDescending(x => x.RequestedBy);
                if (sortTerm.Trim().ToLower().Equals("assigneddate")) sortlist = searchlist.OrderByDescending(x => x.AssignedDate);
                if (sortTerm.Trim().ToLower().Equals("acceptedby")) sortlist = searchlist.OrderByDescending(x => x.AcceptedBy);
                if (sortTerm.Trim().ToLower().Equals("returneddate")) sortlist = searchlist.OrderByDescending(x => x.ReturnedDate);
                if (sortTerm.Trim().ToLower().Equals("returnstate")) sortlist = searchlist.OrderByDescending(x => x.State);
            }
        }

        IEnumerable<ReturnRequestModel> pagelist = sortlist;
        if (pageSize > 0)
        {
            pagelist = sortlist.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return new ReturnRequestPagination
        {
            Requests = pagelist,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = CalculateCeiling(sortlist.Count(), pageSize),
        };
    }

    public ReportPagination GetReportResults(IEnumerable<ReportModel> list, string? sort = "asc", string? sortTerm = "category", int page = 1, int pageSize = 10)
    {
        IEnumerable<ReportModel> sortlist = list;
        if (sort != null && sortTerm != null)
        {
            var sortOrder = sort.Trim().ToLower();
            if (sortOrder.Equals("asc"))
            {
                if (sortTerm.Trim().ToLower().Equals("category")) list = list.OrderBy(x => x.CategoryName);
                if (sortTerm.Trim().ToLower().Equals("total")) list = list.OrderBy(x => x.Total);
                if (sortTerm.Trim().ToLower().Equals("assigned")) list = list.OrderBy(x => x.Assigned);
                if (sortTerm.Trim().ToLower().Equals("available")) list = list.OrderBy(x => x.Available);
                if (sortTerm.Trim().ToLower().Equals("notavailable")) list = list.OrderBy(x => x.Notavailable);
                if (sortTerm.Trim().ToLower().Equals("waitingforrecycling")) list = list.OrderBy(x => x.Waitingforrecycling);
                if (sortTerm.Trim().ToLower().Equals("recycled")) list = list.OrderBy(x => x.Recycled);
            }
            if (sortOrder.Equals("desc"))
            {
                if (sortTerm.Trim().ToLower().Equals("category")) list = list.OrderByDescending(x => x.CategoryName);
                if (sortTerm.Trim().ToLower().Equals("total")) list = list.OrderByDescending(x => x.Total);
                if (sortTerm.Trim().ToLower().Equals("assigned")) list = list.OrderByDescending(x => x.Assigned);
                if (sortTerm.Trim().ToLower().Equals("available")) list = list.OrderByDescending(x => x.Available);
                if (sortTerm.Trim().ToLower().Equals("notavailable")) list = list.OrderByDescending(x => x.Notavailable);
                if (sortTerm.Trim().ToLower().Equals("waitingforrecycling")) list = list.OrderByDescending(x => x.Waitingforrecycling);
                if (sortTerm.Trim().ToLower().Equals("recycled")) list = list.OrderByDescending(x => x.Recycled);
            }
        }

        IEnumerable<ReportModel> pagelist = sortlist;
        if (pageSize > 0)
        {
            pagelist = sortlist.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return new ReportPagination
        {
            Reports = pagelist,
            PageIndex = page,
            PageSize = pageSize,
            TotalPages = CalculateCeiling(sortlist.Count(), pageSize),
        };
    }

    private int CalculateCeiling(int total, int pageSize)
    {
        return (int)Math.Ceiling((double)total / pageSize);
    }
}