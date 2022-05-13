using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Assignments
{
    public class AssignmentCreateModel
    {
        [Required]
        public int AssetId { get; set; }
        [Required]
        public int AssignToId { get; set; }
        public DateTime AssignedDate { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }
        //public string? Location { get; set; }
    }
}