using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Assignments;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IAssignmentService
{
    Task CreateAssignment(AssignmentCreateModel assignment, string adminlocation, int adminid);
    Task UpdateAssignment(AssignmentUpdateModel assignment, string adminlocation, int adminid);
    Task<IEnumerable<AssignmentModel>> GetAllAssignments();
    Task<AssignmentModel> GetAssignment(int id);
    Task DeleteAssignment(int id, string adminlocation);
    Task<IEnumerable<AssignmentModel>> GetAssignmentsByUser(int userid);
    Task UpdateAssignmnetState(int id, string state, int userid);
}

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }

    public async Task CreateAssignment(AssignmentCreateModel assignment, string adminlocation, int adminid)
    {
        await _assignmentRepository.CreateAssignment(_mapper.Map<Assignment>(assignment), adminlocation, adminid);
    }

    public async Task UpdateAssignment(AssignmentUpdateModel assignment, string adminlocation, int adminid)
    {
        await _assignmentRepository.UpdateAssignment(assignment, adminlocation, adminid);
    }

    public async Task<IEnumerable<AssignmentModel>> GetAllAssignments()
    {
        return _mapper.Map<IEnumerable<AssignmentModel>>(await _assignmentRepository.GetAllAssignments());
    }

    public async Task<AssignmentModel> GetAssignment(int id)
    {
        return _mapper.Map<AssignmentModel>(await _assignmentRepository.GetAssignment(id));
    }

    public async Task DeleteAssignment(int id, string adminlocation)
    {
        await _assignmentRepository.DeleteAssignment(id, adminlocation);
    }

    public async Task<IEnumerable<AssignmentModel>> GetAssignmentsByUser(int userid)
    {
        return _mapper.Map<IEnumerable<AssignmentModel>>(await _assignmentRepository.GetAssignmentsByUser(userid));
    }

    public async Task UpdateAssignmnetState(int id, string state, int userid)
    {
        await _assignmentRepository.UpdateAssignmnetState(id, state, userid);
    }

}