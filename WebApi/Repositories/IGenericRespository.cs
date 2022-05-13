
using WebApi.Entities;

namespace WebApi.Repositories;

public interface IGenericRespository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> GetByIdAsync(int id);
    public Task CreateAsync(T entity, int adminid);
    public Task EditAsync(T entity);
    public Task<IEnumerable<T>> GetListByPageAsync(int page, int pageSize);
    public Task DeleteAsync(int id);
    public Task TransactAsync(Action callback);
}