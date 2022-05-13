using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.Models.Report
{
    public class ReportModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int Total { get; set; }
        public int Assigned { get; set; }
        public int Available { get; set; }
        public int Notavailable { get; set; }
        public int Waitingforrecycling { get; set; }
        public int Recycled { get; set; }
    }
}