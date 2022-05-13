namespace WebApi.Repositories;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Assignments;

public interface IAssignmentRepository
{
    Task CreateAssignment(Assignment assignment, string adminlocation, int adminid);
    Task UpdateAssignment(AssignmentUpdateModel assignment, string adminlocation, int adminid);
    Task<IEnumerable<Assignment>> GetAllAssignments();// need location to secure bypass
    Task<Assignment> GetAssignment(int id);// need location
    Task DeleteAssignment(int id, string adminlocation); // need location
    Task<IEnumerable<Assignment>> GetAssignmentsByUser(int userid);
    Task UpdateAssignmnetState(int id, string state, int userid);
}

public class AssignmentRepository : IAssignmentRepository
{
    private readonly DataContext _context;
    public AssignmentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task CreateAssignment(Assignment assignment, string adminlocation, int adminid)
    {
        var asset = await _context.Assets.FindAsync(assignment.AssetId);
        var user = await _context.Users.FindAsync(assignment.AssignToId);

        if (user == null || user.IsDisabled == true) throw new AppException("User does not exist or is disabled");
        if (!CheckAssignedDate(assignment.AssignedDate)) throw new AppException("Assigned date is not valid");
        if (!CheckUserLocation(assignment.AssignToId, adminlocation)) throw new AppException("User to assign does not exist or is not in your location");
        if (!CheckAvailableAsset(assignment.AssetId)) throw new AppException("Asset is not available");
        if (!CheckAssetLocation(assignment.AssetId, adminlocation)) throw new AppException("Asset is not in your location");
        if (asset == null) throw new AppException("Asset does not exist");

        var assignmentEntity = new Assignment
        {
            AssetId = assignment.AssetId,
            AssignToId = assignment.AssignToId,
            AssignedDate = assignment.AssignedDate,
            AssignedById = adminid,
            Note = assignment.Note?.Trim(),
            State = AssignmentState.WaitingForAcceptance,
            Location = adminlocation,
            IsInProgress = true,
            CreateAt = DateTime.Now,
            CreateBy = adminid,
            UpdateAt = null,
            UpdateBy = null,
        };

        // update asset state
        asset.State = AssetState.Assigned;
        _context.Assets.Update(asset);

        // add assignment and save change database
        await _context.Assignments.AddAsync(assignmentEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAssignment(AssignmentUpdateModel assignment, string adminlocation, int adminid)
    {
        var assignmentEntity = await _context.Assignments.FindAsync(assignment.Id);
        var oldasset = await _context.Assets.FindAsync(assignmentEntity?.AssetId);
        var newasset = await _context.Assets.FindAsync(assignment.AssetId);
        var user = await _context.Users.FindAsync(assignment.AssignToId);

        // check conditions and return message
        if (user == null || user.IsDisabled == true) throw new AppException("User does not exist or is disabled");
        if (!CheckAssignedDate(assignment.AssignedDate)) throw new AppException("Assigned date is not valid");
        if (!CheckUserLocation(assignment.AssignToId, adminlocation)) throw new AppException("User to assign does not exist or is not in your location");
        if (!CheckAvailableAsset(assignment.AssetId) && (assignmentEntity?.AssetId != assignment.AssetId)) throw new AppException("Asset is not available");
        if (!CheckAssetLocation(assignment.AssetId, adminlocation)) throw new AppException("Asset is not in your location");
        if (assignmentEntity == null) throw new AppException("Assignment not found");
        if (!assignmentEntity.State.Equals(AssignmentState.WaitingForAcceptance)) throw new AppException("Only waiting for acceptance assignment can be updated");
        if (!assignmentEntity.IsInProgress) throw new AppException("Assignment has already been completed");
        //if (assignmentEntity.AssignedById != adminid) throw new AppException("You are not allowed to update this assignment");
        if (oldasset == null) throw new AppException("Old asset not found");
        if (newasset == null) throw new AppException("New asset to assign not found");

        //restore old asset state to available
        oldasset.State = AssetState.Available;
        _context.Assets.Update(oldasset);

        //update new information to assignment
        assignmentEntity.AssignToId = assignment.AssignToId;
        assignmentEntity.AssignedById = adminid;
        assignmentEntity.AssetId = assignment.AssetId;
        assignmentEntity.AssignedDate = assignment.AssignedDate;
        assignmentEntity.Note = assignment.Note?.Trim();
        assignmentEntity.UpdateAt = DateTime.Now;
        assignmentEntity.UpdateBy = adminid;

        // change state of new asset to assigned
        newasset.State = AssetState.Assigned;
        _context.Assets.Update(newasset);

        //update assignment and save change
        _context.Assignments.Update(assignmentEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Assignment>> GetAllAssignments()
    {
        return await _context.Assignments
        .Where(a => (a.IsInProgress == true) || a.State.Equals(AssignmentState.Declined))
        .Include(a => a.Asset)
        .Include(a => a.AssignTo)
        .Include(a => a.AssignedBy)
        .ToListAsync();
    }

    public async Task<Assignment> GetAssignment(int id)
    {
        var assignment = await _context.Assignments.Include(a => a.Asset).Include(a => a.AssignTo).Include(a => a.AssignedBy).FirstOrDefaultAsync(a => a.Id == id);
        if (assignment == null) throw new AppException("Assignment not found");
        return assignment;
    }

    public async Task DeleteAssignment(int id, string adminlocaton)
    {
        var assignment = await _context.Assignments.FindAsync(id);

        if (assignment == null) throw new AppException("Assignment not found");
        if (!assignment.Location.Equals(adminlocaton)) throw new AppException("You are not allowed to delete this assignment");
        if (assignment.State.Equals(AssignmentState.Accepted)) throw new AppException("Can not delete accepted assignment");
        if (assignment.State.Equals(AssignmentState.WaitingForAcceptance))
        {
            var asset = await _context.Assets.FindAsync(assignment.AssetId);
            if (asset == null) throw new AppException("Asset not found");
            if (asset.State.Equals(AssetState.Assigned))
            {
                asset.State = AssetState.Available;
                _context.Assets.Update(asset);
                _context.Assignments.Remove(assignment);
            }
        }
        if (assignment.State.Equals(AssignmentState.Declined) && assignment.IsInProgress == false)
        {
            _context.Assignments.Remove(assignment);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Assignment>> GetAssignmentsByUser(int userid)
    {
        var assignments = await _context.Assignments.Where(a => a.AssignToId == userid && a.IsInProgress == true).Include(a => a.Asset).Include(a => a.AssignTo).Include(a => a.AssignedBy).ToListAsync();
        var assignmentsByDate = assignments.Where(a => DateTime.Compare(a.AssignedDate.Date, DateTime.Now.Date) <= 0).ToList();
        return assignmentsByDate;
    }

    public async Task UpdateAssignmnetState(int id, string state, int userid) // assignment id, state to change , id of assigned user
    {
        var assignment = await _context.Assignments.FindAsync(id);

        if (assignment == null) throw new AppException("Assignment not found");

        var asset = await _context.Assets.FindAsync(assignment.AssetId);
        if (asset == null) throw new AppException("Asset in this assignment not found");
        if (assignment.AssignToId != userid) throw new AppException("You are not assigned to this assignment");
        if (assignment.State.Equals(AssignmentState.Accepted)) throw new AppException("Assignment already accepted");
        if (assignment.State.Equals(AssignmentState.Declined)) throw new AppException("Assignment already declined");
        if (assignment.AssignToId == userid)
        {
            if (state.Trim().ToLower().Equals("accepted"))
            {
                assignment.State = AssignmentState.Accepted;
                assignment.UpdateAt = DateTime.Now;
                assignment.UpdateBy = userid;
                _context.Assignments.Update(assignment);
            }
            else
            {
                assignment.State = AssignmentState.Declined;
                assignment.IsInProgress = false;
                assignment.UpdateAt = DateTime.Now;
                assignment.UpdateBy = userid;
                _context.Assignments.Update(assignment);

                asset.State = AssetState.Available;
                _context.Assets.Update(asset);

                //_context.Assignments.Remove(assignment);
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new AppException("Permission are not allowed");
        }

    }

    private bool CheckAssignedDate(DateTime date)
    {
        if (DateTime.Compare(date.Date, (DateTime.Now).Date) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckUserLocation(int userId, string location)
    {
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null) throw new AppException("User not found");
        if (user.Location == null) throw new AppException("User location not found");
        if (user != null && user.Location.Equals(location))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckAvailableAsset(int? assetId)
    {
        var check = _context.Assets.SingleOrDefault(a => a.Id == assetId)?.State;
        if (check == AssetState.Available) return true;
        return false;
    }

    private bool CheckAssetLocation(int? assetId, string location)
    {
        var check = _context.Assets.SingleOrDefault(a => a.Id == assetId)?.Location;
        if (check == null) throw new AppException("Asset location not found");
        if (check.Equals(location)) return true;
        return false;
    }
}