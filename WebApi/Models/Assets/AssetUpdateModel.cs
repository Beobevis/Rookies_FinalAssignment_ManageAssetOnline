using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Assets
{
    public class AssetUpdateModel
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? AssetName { get; set; }
        [StringLength(500)]
        public string Specification { get; set; } = null!;
        public DateTime InstalledDate { get; set; }
        public string? State { get; set; }
    }
}