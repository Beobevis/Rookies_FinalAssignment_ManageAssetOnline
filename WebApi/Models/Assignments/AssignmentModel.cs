using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.Models.Assignments
{
    public class AssignmentModel
    {
        public int Id { get; set; }
        [Required]
        public int AssetId { get; set; }
        public string AssetCode { get; set; } = null!;
        public string AssetName { get; set; } = null!;
        [Required]
        public int AssignToId { get; set; }
        public string AssignTo { get; set; } = null!;
        public string AssignToFirstname { get; set; } = null!;
        public string AssignToLastname { get; set; } = null!;
        [Required]
        public int AssignedById { get; set; }
        public string? AssignedBy { get; set; }
        [StringLength(500)]
        public string? Specification { get; set; }
        public DateTime AssignedDate { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }
        public AssignmentState State { get; set; }
        public bool IsInProgress { get; set; }
        public string Location { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
    }
}