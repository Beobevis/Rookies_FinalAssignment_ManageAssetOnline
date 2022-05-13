using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.Models.Assignments
{
    public class HistoricalAssignmentModel
    {
        public int Id { get; set; }
        //public int AssetId { get; set; }
        //public string? AssetCode { get; set; }
        //public string? AssetName { get; set; }
        //public int AssignToId { get; set; }
        public string? AssignTo { get; set; }
        //public int AssignedById { get; set; }
        public string? AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public string? Note { get; set; }

    }
}