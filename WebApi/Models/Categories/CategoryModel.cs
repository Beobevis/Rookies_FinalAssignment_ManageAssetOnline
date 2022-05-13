using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Categories
{
    public class CategoryModel : BaseModel
    {
        public int? Id { get; set; } = null; // instance null
        [Required]
        public string? CategoryName { get; set; }
        [Required, StringLength(5)]
        public string? Prefix { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
    }
}