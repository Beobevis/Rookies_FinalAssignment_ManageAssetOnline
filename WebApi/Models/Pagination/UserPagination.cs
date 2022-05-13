using WebApi.Entities;

namespace WebApi.Models.Pagination
{
    public class UserPagination
    {
        public IEnumerable<User> Users { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}