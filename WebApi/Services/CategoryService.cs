
using WebApi.Models.Categories;
using AutoMapper;
using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services;

public class CategoryService : IGenericService<CategoryModel>
{

    private readonly IMapper _mapper;
    private readonly IGenericRespository<Category> _repository;

    public CategoryService(IMapper mapper, IGenericRespository<Category> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task CreateAsync(CategoryModel category, int adminid)
    {
        await _repository.CreateAsync(_mapper.Map<Category>(category), adminid);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task EditAsync(CategoryModel category)
    {
        await _repository.EditAsync(_mapper.Map<Category>(category));
    }

    public async Task<IEnumerable<CategoryModel>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<CategoryModel>>(await _repository.GetAllAsync());
    }

    public async Task<CategoryModel> GetByIdAsync(int id)
    {
        return _mapper.Map<CategoryModel>(await _repository.GetByIdAsync(id));
    }

    public async Task<IEnumerable<CategoryModel>> GetListByPageAsync(int page, int pageSize)
    {
        return _mapper.Map<IEnumerable<CategoryModel>>(await _repository.GetListByPageAsync(page, pageSize));
    }
}