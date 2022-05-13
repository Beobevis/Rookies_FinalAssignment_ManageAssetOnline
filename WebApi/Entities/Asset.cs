using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Entities;

public class Asset : BaseEntity {
    [Required]
    public string? AssetCode { get; set; }
    [Required,StringLength(50)]
    public string? AssetName { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    [Required,StringLength(500)]
    public string Specification { get; set; } = null!;
    [Required]
    public DateTime InstalledDate { get; set; }
    [Required]
    public string Location { get; set; } = null!;
    [Required]
    public AssetState State { get; set; }
    public DateTime CreateAt { get; set; }
    public int CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }
    //[JsonIgnore]
    public ICollection<Assignment> Assignments { get; set; } = null!;
    [JsonIgnore]
    public ICollection<ReturnRequest>? ReturnRequests { get; set; }
}