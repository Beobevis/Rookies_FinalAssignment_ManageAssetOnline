namespace WebApi.Repositories;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;

public interface IReturnRepository
{
    Task CreateReturnRequestUser(int assignmentid, string userlocation, int userid);
    Task CreateReturnRequestAdmin(int assignmentid, string adminlocation, int adminid);
    Task<IEnumerable<ReturnRequest>> GetAllReturnRequests();
    Task<ReturnRequest> GetReturnRequest(int id);
    Task DeleteReturnRequest(int id, string adminlocation);
    Task AcceptReturnRequest(int requestid, int adminid, string adminlocation);
}

public class ReturnRepository : IReturnRepository
{
    private readonly DataContext _context;
    public ReturnRepository(DataContext context)
    {
        _context = context;
    }

    public async Task CreateReturnRequestUser(int assignmentid, string userlocation, int userid)
    {
        var assignment = await _context.Assignments.FindAsync(assignmentid);
        var asset = await _context.Assets.FindAsync(assignment?.AssetId);

        if (assignment == null) throw new AppException("Assignment not found");
        var requestcount = await _context.ReturnRequests.Where(r => r.AssetId == assignment.AssetId && r.State == ReturnState.WaitingForReturn).CountAsync();

        if (requestcount > 0) throw new AppException("Asset is already waiting for return");
        if (asset == null) throw new AppException("Asset not found");
        if (assignment.Location == null) throw new AppException("Assignment location not found");
        if (assignment.IsInProgress == false) throw new AppException("Assignment is not in progress, this is a historical assignment");
        if (assignment.AssignToId != userid) throw new AppException("Only assignee of this assignment can make return request");
        if (!assignment.Location.Equals(userlocation)) throw new AppException("Only user with same location with assignment can make return request");
        if (!asset.State.Equals(AssetState.Assigned)) throw new AppException("Asset is not assigned");
        if (!assignment.State.Equals(AssignmentState.Accepted)) throw new AppException("Assignment is not accepted, can not make return request");

        var returnEntity = new ReturnRequest();
        returnEntity.AssetId = assignment.AssetId;
        returnEntity.RequestedById = userid;
        returnEntity.Location = userlocation;
        returnEntity.State = ReturnState.WaitingForReturn;
        returnEntity.AssignedDate = assignment.AssignedDate;
        returnEntity.CreateAt = DateTime.Now;
        returnEntity.CreateBy = userid;
        returnEntity.UpdateAt = null;
        returnEntity.UpdateBy = null;

        await _context.ReturnRequests.AddAsync(returnEntity);
        await _context.SaveChangesAsync();
    }

    public async Task CreateReturnRequestAdmin(int assignmentid, string adminlocation, int adminid)
    {
        var assignment = await _context.Assignments.FindAsync(assignmentid);
        var asset = await _context.Assets.FindAsync(assignment?.AssetId);

        if (assignment == null) throw new AppException("Assignment not found");
        var requestcount = await _context.ReturnRequests.Where(r => r.AssetId == assignment.AssetId && r.State == ReturnState.WaitingForReturn).CountAsync();

        if (requestcount > 0) throw new AppException("Asset is already waiting for return");
        if (asset == null) throw new AppException("Asset not found");
        if (assignment.Location == null) throw new AppException("Assignment location not found");
        if (assignment.IsInProgress == false) throw new AppException("Assignment is not in progress, this is a historical assignment");
        if (assignment.State != AssignmentState.Accepted) throw new AppException("Assignment is not accepted, can not make return request");
        if (asset.State != AssetState.Assigned) throw new AppException("Asset is not assigned");
        if (!assignment.Location.Equals(adminlocation)) throw new AppException("Only admin with same location with assignment can make return request");

        var returnEntity = new ReturnRequest();
        returnEntity.AssetId = assignment.AssetId;
        returnEntity.RequestedById = assignment.AssignToId;
        returnEntity.Location = adminlocation;
        returnEntity.State = ReturnState.WaitingForReturn;
        returnEntity.AssignedDate = assignment.AssignedDate;
        returnEntity.CreateAt = DateTime.Now;
        returnEntity.CreateBy = adminid;
        returnEntity.UpdateAt = null;
        returnEntity.UpdateBy = null;

        await _context.ReturnRequests.AddAsync(returnEntity);
        await _context.SaveChangesAsync();

    }

    public async Task<IEnumerable<ReturnRequest>> GetAllReturnRequests()
    {
        return await _context.ReturnRequests.Include(r => r.Asset).Include(r => r.RequestedBy).Include(r => r.AcceptedBy).ToListAsync();
    }

    public async Task<ReturnRequest> GetReturnRequest(int id)
    {
        var request = await _context.ReturnRequests.Include(r => r.Asset).Include(r => r.RequestedBy).Include(r => r.AcceptedBy).SingleOrDefaultAsync(r => r.Id == id);
        
        if (request == null) throw new AppException("Return request not found");
        return request;
    }

    public async Task DeleteReturnRequest(int id, string adminlocation)
    {
        var request = await _context.ReturnRequests.FindAsync(id);

        if (request == null) throw new AppException("Return request not found");
        if (!request.Location.Equals(adminlocation)) throw new AppException("Only admin with same location with return request can delete return request");
        if (request.State == ReturnState.Completed) throw new AppException("Return request is already completed");

        _context.ReturnRequests.Remove(request);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptReturnRequest(int requestid, int adminid, string adminlocation)
    {
        var request = await _context.ReturnRequests.FindAsync(requestid);

        if (request == null) throw new AppException("Return request not found");
        if (!request.Location.Equals(adminlocation)) throw new AppException("Only admin with same location with request can accept request");

        var asset = await _context.Assets.FindAsync(request.AssetId);
        var assignment = await _context.Assignments.SingleOrDefaultAsync(a => a.AssetId == request.AssetId && a.IsInProgress == true);

        if (assignment == null) throw new AppException("Assignment not found");
        if (asset == null) throw new AppException("Asset not found");
        if (asset.State != AssetState.Assigned) throw new AppException("Asset is not assigned, can not be return");
        if (request.State != ReturnState.WaitingForReturn) throw new AppException("Only waiting for return request can be accepted");
        if (request.AcceptedById != null || request.ReturnedDate != null) throw new AppException("Return request is already accepted");

        request.AcceptedById = adminid;
        request.ReturnedDate = DateTime.Now;
        request.State = ReturnState.Completed;
        request.UpdateAt = DateTime.Now; // optional
        request.UpdateBy = adminid;  // optional

        asset.State = AssetState.Available;

        assignment.IsInProgress = false;

        _context.Assignments.Update(assignment);
        _context.Assets.Update(asset);
        _context.ReturnRequests.Update(request);
        await _context.SaveChangesAsync();
    }

}