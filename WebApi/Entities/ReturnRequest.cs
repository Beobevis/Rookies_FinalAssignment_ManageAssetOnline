using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities;

public class ReturnRequest : BaseEntity {
    [Required]
    public int? AssetId { get; set; }
    public virtual Asset? Asset { get; set; }
    [Required]
    public int RequestedById { get; set; }
    public virtual User RequestedBy { get; set; } = null!; // consider nullable
    public int? AcceptedById { get; set; }
    public virtual User? AcceptedBy { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    [Required]
    public ReturnState State { get; set; }
    [Required]
    public string Location { get; set; } = null!;
    public DateTime CreateAt { get; set; }
    public int CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }
}