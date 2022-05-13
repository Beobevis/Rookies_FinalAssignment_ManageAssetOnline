
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Models.Assignments;

namespace WebApi.Models.Assets
{
    public class AssetModel
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = null!;
        [StringLength(50)]
        public string AssetName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        [StringLength(500)]
        public string Specification { get; set; } = null!;
        public DateTime InstalledDate { get; set; }
        public string State { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        //[JsonIgnore]
        public ICollection<HistoricalAssignmentModel>? Assignments { get; set; }
    }
}