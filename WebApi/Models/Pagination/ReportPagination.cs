using WebApi.Models.Report;

namespace WebApi.Models.Pagination
{
    public class ReportPagination
    {
        public IEnumerable<ReportModel> Reports { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}