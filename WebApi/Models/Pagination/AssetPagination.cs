
using WebApi.Models.Assets;

namespace WebApi.Models.Pagination
{
    public class AssetPagination
    {
        public IEnumerable<AssetModel> Assets { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}