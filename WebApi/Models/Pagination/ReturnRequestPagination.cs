using WebApi.Models.ReturnRequest;

namespace WebApi.Models.Pagination
{
    public class ReturnRequestPagination
    {
        public IEnumerable<ReturnRequestModel> Requests { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}