

using WebApi.Models;

namespace WebApi.Services;

public interface IGenericService<T> where T : BaseModel
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> GetByIdAsync(int id);
    public Task CreateAsync(T model, int adminid);
    public Task EditAsync(T model);
    public Task<IEnumerable<T>> GetListByPageAsync(int page, int pageSize);
    public Task DeleteAsync(int id);
}