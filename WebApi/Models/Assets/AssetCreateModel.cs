using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Assets
{
    public class AssetCreateModel
    {
        [Required,StringLength(50)]
        public string? AssetName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required,StringLength(500)]
        public string Specification { get; set; } = null!;
        public DateTime InstalledDate { get; set; }
        public string State { get; set; } = null!;
    }
}