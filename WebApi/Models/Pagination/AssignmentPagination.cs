using WebApi.Models.Assignments;

namespace WebApi.Models.Pagination
{
    public class AssignmentPagination
    {
        public IEnumerable<AssignmentModel> Assignments { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}