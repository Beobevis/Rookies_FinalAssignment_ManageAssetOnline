namespace WebApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Category : BaseEntity {
    [Required]
    public string CategoryName { get; set; } = null!;
    [Required,StringLength(5)]
    public string Prefix { get; set; } = null!;
    public DateTime CreateAt { get; set; }
    public int CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }
    [JsonIgnore]
    public ICollection<Asset>? Assets { get; set; }
}
