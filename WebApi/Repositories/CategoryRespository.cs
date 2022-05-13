using WebApi.Entities;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi.Repositories
{
    public class CategoryRespository : IGenericRespository<Category>
    {
        private readonly DataContext _context;
        private readonly IDbContextTransaction? _transaction;

        public CategoryRespository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category entity, int adminid)
        {
            var checkName = await _context.Categories.AnyAsync(
                x => x.CategoryName.Equals(entity.CategoryName)
            );
            if (checkName)
                throw new AppException("Category Name is already used");
            var checkprefix = await _context.Categories.AnyAsync(
                x => x.Prefix.Equals(entity.Prefix)
            );
            if (checkprefix)
                throw new AppException("Prefix is already used");
            entity.Prefix = entity.Prefix.ToUpper().Trim();
            entity.CreateAt = DateTime.Now;
            entity.CreateBy = adminid;
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");
            await TransactAsync(
                () =>
                {
                    _context.Categories.Remove(category);
                }
            );
        }

        public async Task EditAsync(Category entity)
        {
            var editCategory = await _context.Categories.FindAsync(entity.Id);
            if (editCategory == null)
                throw new KeyNotFoundException("Category not found");
            await TransactAsync(
                () =>
                {
                    var checkprefix = _context.Categories.FirstOrDefault(
                        x => x.Prefix.Equals(entity.Prefix)
                    );
                    if (checkprefix != null)
                        throw new AppException("Prefix is already used");
                    editCategory.UpdateAt = DateTime.Now; // optional
                    editCategory.UpdateBy = null; // optional
                    _context.Categories.Update(editCategory);
                    _context.SaveChangesAsync();
                }
            );
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");
            return category;
        }

        public async Task<IEnumerable<Category>> GetListByPageAsync(int page, int pageSize)
        {
            return await _context.Categories.Skip(page - 1 * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task TransactAsync(Action callback)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                callback();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
