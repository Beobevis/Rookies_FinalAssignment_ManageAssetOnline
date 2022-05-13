using AutoMapper;
using WebApi.Models.ReturnRequest;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IReturnService
{
    Task CreateReturnRequestUser(int assignmentid, string userlocation, int userid);
    Task CreateReturnRequestAdmin(int assignmentid, string adminlocation, int adminid);
    Task<IEnumerable<ReturnRequestModel>> GetAllReturnRequests();
    Task<ReturnRequestModel> GetReturnRequest(int id);
    Task DeleteReturnRequest(int id, string adminlocation);
    Task AcceptReturnRequest(int requestid, int adminid, string adminlocation);
}

public class ReturnService : IReturnService
{
    private readonly IReturnRepository _returnRepository;
    private readonly IMapper _mapper;
    public ReturnService(IReturnRepository returnRepository, IMapper mapper)
    {
        _returnRepository = returnRepository;
        _mapper = mapper;
    }

    public async Task CreateReturnRequestUser(int assignmentid, string userlocation, int userid)
    {
        await _returnRepository.CreateReturnRequestUser(assignmentid, userlocation, userid);
    }
    public async Task CreateReturnRequestAdmin(int assignmentid, string adminlocation, int adminid)
    {
        await _returnRepository.CreateReturnRequestAdmin(assignmentid, adminlocation, adminid);
    }
    public async Task<IEnumerable<ReturnRequestModel>> GetAllReturnRequests()
    {
        return _mapper.Map<IEnumerable<ReturnRequestModel>>(await _returnRepository.GetAllReturnRequests());
    }
    public async Task<ReturnRequestModel> GetReturnRequest(int id)
    {
        return _mapper.Map<ReturnRequestModel>(await _returnRepository.GetReturnRequest(id));
    }
    public async Task DeleteReturnRequest(int id, string adminlocation)
    {
        await _returnRepository.DeleteReturnRequest(id, adminlocation);
    }
    public async Task AcceptReturnRequest(int requestid, int adminid, string adminlocation)
    {
        await _returnRepository.AcceptReturnRequest(requestid, adminid, adminlocation);
    }

}