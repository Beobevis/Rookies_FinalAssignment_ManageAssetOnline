using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.Models.ReturnRequest
{
    public class ReturnRequestModel
    {
        public int Id { get; set; }
        [Required]
        public int AssetId { get; set; }
        public string AssetCode { get; set; } = null!;
        public string AssetName { get; set; } = null!;
        [Required]
        public int RequestedById { get; set; }
        public string RequestedBy { get; set; } = null!;
        public int? AcceptedById { get; set; }
        public string? AcceptedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        [Required]
        public ReturnState State { get; set; }
        [Required]
        public string Location { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
    }
}